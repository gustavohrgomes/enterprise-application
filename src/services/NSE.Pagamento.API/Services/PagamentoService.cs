using FluentValidation.Results;
using NSE.Core.Messages.IntegrationEvents;
using NSE.Pagamentos.API.Facade;
using NSE.Pagamentos.API.Models;

namespace NSE.Pagamentos.API.Services;

public class PagamentoService : IPagamentoService
{
    private readonly IPagamentoFacade _pagamentoFacade;
    private readonly IPagamentoRepository _pagamentoRepository;

    public PagamentoService(IPagamentoFacade pagamentoFacade, IPagamentoRepository pagamentoRepository)
    {
        _pagamentoFacade = pagamentoFacade ?? throw new ArgumentNullException(nameof(pagamentoFacade));
        _pagamentoRepository = pagamentoRepository ?? throw new ArgumentNullException(nameof(pagamentoRepository));
    }

    public async Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento)
    {
        var transacao = await _pagamentoFacade.AutorizarPagamento(pagamento);
        var validationResult = new ValidationResult();

        if (transacao.Status != StatusTransacao.Autorizado)
        {
            validationResult.Errors.Add(new ValidationFailure("Pagamento", "Pagamento Recusado, entre em contato com a sua operadora de cartão"));

            return new ResponseMessage(validationResult);
        }

        pagamento.AdicionarTransacao(transacao);
        _pagamentoRepository.AdicionarPagamento(pagamento);

        if (!await _pagamentoRepository.UnitOfWork.CommitAsync())
        {
            validationResult.Errors.Add(new ValidationFailure("Pagamento", "Houve um erro ao realizar o pagamento"));

            // TODO: Comunicar com o gateway para realizar o estorno

            return new ResponseMessage(validationResult);
        }

        return new ResponseMessage(validationResult);
    }
}
