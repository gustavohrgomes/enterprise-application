﻿using NSE.Core.DomainObjects;

namespace NSE.Pagamentos.API.Models;

public class Pagamento : AggregateRoot
{
    public Pagamento()
    {
        Transacoes = new List<Transacao>();
    }

    public Guid PedidoId { get; set; }
    public TipoPagamento TipoPagamento { get; set; }
    public decimal Valor { get; set; }

    public CartaoCredito CartaoCredito { get; set; }

    public ICollection<Transacao> Transacoes { get; set; }

    public void AdicionarTransacao(Transacao transacao)
        => Transacoes.Add(transacao);
}