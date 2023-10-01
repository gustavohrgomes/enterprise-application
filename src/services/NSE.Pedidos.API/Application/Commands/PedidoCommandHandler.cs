using MassTransit;
using MediatR;
using NSE.Core.Data;
using NSE.Core.Messages;
using NSE.Core.Messages.IntegrationEvents;
using NSE.MessageBus;
using NSE.Pedidos.API.Application.DTO;
using NSE.Pedidos.API.Application.Events;
using NSE.Pedidos.Domain.Pedidos;
using NSE.Pedidos.Domain.Specs;
using NSE.Pedidos.Domain.Vouchers;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace NSE.Pedidos.API.Application.Commands;

public class PedidoCommandHandler : CommandHandler, IRequestHandler<AdicionarPedidoCommand, ValidationResult>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IVoucherRepository _voucherRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRequestClient<PedidoIniciadoIntegrationEvent> _requestClient;

    public PedidoCommandHandler(IVoucherRepository voucherRepository, 
                                IPedidoRepository pedidoRepository,
                                IUnitOfWork unitOfWork,
                                IRequestClient<PedidoIniciadoIntegrationEvent> requestClient)
    {
        _pedidoRepository = pedidoRepository ?? throw new ArgumentNullException(nameof(pedidoRepository));
        _voucherRepository = voucherRepository ?? throw new ArgumentNullException(nameof(voucherRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _requestClient = requestClient ?? throw new ArgumentNullException(nameof(requestClient));
    }

    public async Task<ValidationResult> Handle(AdicionarPedidoCommand message, CancellationToken cancellationToken)
    {
        if (!message.EhValido()) return message.ValidationResult;

        var pedido = MapearPedido(message);

        if (!await AplicarVoucher(message, pedido)) return ValidationResult;

        if (!ValidarPedido(pedido)) return ValidationResult;

        if (!await ProcessarPagamento(pedido, message)) return ValidationResult;

        pedido.AutorizarPedido();

        pedido.AddDomainEvent(new PedidoRealizadoEvent(pedido.Id, pedido.ClienteId));

        _pedidoRepository.Adicionar(pedido);

        await _unitOfWork.ResilientCommitAsync(cancellationToken);

        return ValidationResult;
    }

    private static Pedido MapearPedido(AdicionarPedidoCommand message)
    {
        var endereco = new Endereco
        {
            Logradouro = message.Endereco.Logradouro,
            Numero = message.Endereco.Numero,
            Complemento = message.Endereco.Complemento,
            Bairro = message.Endereco.Bairro,
            Cep = message.Endereco.Cep,
            Cidade = message.Endereco.Cidade,
            Estado = message.Endereco.Estado
        };

        var pedido = new Pedido(
            Guid.NewGuid(),
            message.ClienteId,
            message.ValorTotal,
            message.PedidoItems.Select(PedidoItemDTO.ParaPedidoItem).ToList(),
            message.VoucherUtilizado, 
            message.Desconto);

        pedido.AtribuirEndereco(endereco);
        return pedido;
    }

    private async Task<bool> AplicarVoucher(AdicionarPedidoCommand message, Pedido pedido)
    {
        if (!message.VoucherUtilizado) return true;

        var voucher = await _voucherRepository.ObterVoucherPorCodigo(message.VoucherCodigo);
        if (voucher is null)
        {
            AddProcessingError("O voucher informado não existe!");
            return false;
        }

        var voucherValidation = new VoucherValidation().Validate(voucher);
        if (!voucherValidation.IsValid)
        {
            voucherValidation.Errors.ToList().ForEach(m => AddProcessingError(m.ErrorMessage));
            return false;
        }

        pedido.AtribuirVoucher(voucher);
        voucher.DebitarQuantidade();

        _voucherRepository.Atualizar(voucher);

        return true;
    }

    private bool ValidarPedido(Pedido pedido)
    {
        var valorOriginalPedido = pedido.ValorTotal;
        var descontoOriginalPedido = pedido.Desconto;

        pedido.CalcularValorPedido();

        if (pedido.ValorTotal != valorOriginalPedido)
        {
            AddProcessingError("O valor total do pedido não confere com o cálculo do pedido");
            return false;
        }

        if (pedido.Desconto != descontoOriginalPedido)
        {
            AddProcessingError("O valor total não confere com o cálculo do pedido");
            return false;
        }

        return true;
    }

    private async Task<bool> ProcessarPagamento(Pedido pedido, AdicionarPedidoCommand message)
    {
        var pedidoIniciado = new PedidoIniciadoIntegrationEvent
        {
            PedidoId = pedido.Id,
            ClienteId = pedido.ClienteId,
            Valor = pedido.ValorTotal,
            TipoPagamento = 1, // fixo. Alterar se tiver mais tipos
            NomeCartao = message.NomeCartao,
            NumeroCartao = message.NumeroCartao,
            MesAnoVencimento = message.ExpiracaoCartao,
            CVV = message.CvvCartao
        };

        var result = await _requestClient.GetResponse<ResponseMessage>(pedidoIniciado);

        if (result.Message.ValidationResult.IsValid) return true;

        foreach (var erro in result.Message.ValidationResult.Errors)
        {
            AddProcessingError(erro.ErrorMessage);
        }

        return false;
    }
}
