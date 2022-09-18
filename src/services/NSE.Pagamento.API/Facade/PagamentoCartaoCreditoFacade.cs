using Microsoft.Extensions.Options;
using NSE.Pagamentos.API.Models;
using NSE.Pagamentos.NerdsPag;

namespace NSE.Pagamentos.API.Facade;

public class PagamentoCartaoCreditoFacade : IPagamentoFacade
{
    private readonly PagamentoConfig _pagamentoConfig;

    public PagamentoCartaoCreditoFacade(IOptions<PagamentoConfig> pagamentoConfig)
    {
        _pagamentoConfig = pagamentoConfig.Value;
    }

    public async Task<Transacao> AutorizarPagamento(Pagamento pagamento)
    {
        var nerdsPagSvc = new NerdsPagService(_pagamentoConfig.DefaultApiKey!, _pagamentoConfig.DefaultApiKey!);

        var cardHashGen = new CardHash(nerdsPagSvc)
        {
            CardHolderName = pagamento.CartaoCredito.NomeCartao,
            CardNumber = pagamento.CartaoCredito.NumeroCartao,
            CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento,
            CardCvv = pagamento.CartaoCredito.CVV,
        };

        var cardHash = cardHashGen.Generate();

        var transaction = new Transaction(nerdsPagSvc)
        {
            CardHash = cardHash,
            CardNumber = pagamento.CartaoCredito.NumeroCartao,
            CardHolderName = pagamento.CartaoCredito.NomeCartao,
            CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento,
            CardCvv = pagamento.CartaoCredito.CVV,
            PaymentMethod = PaymentMethod.CreditCard,
            Amount = pagamento.Valor
        };

        return ParaTransacao(await transaction.AuthorizeCardTransaction());
    }

    public static Transacao ParaTransacao(Transaction transaction)
    {
        return new Transacao
        {
            Id = Guid.NewGuid(),
            Status = (StatusTransacao)transaction.Status,
            ValorTotal = transaction.Amount,
            BandeiraCartao = transaction.CardBrand,
            CodigoAutorizacao = transaction.AuthorizationCode,
            CustoTransacao = transaction.Cost,
            DataTransacao = transaction.TransactionDate,
            NSU = transaction.Nsu,
            TID = transaction.Tid
        };
    }
}
