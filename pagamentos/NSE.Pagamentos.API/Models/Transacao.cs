using NSE.Core.DomainObjects;

namespace NSE.Pagamentos.API.Models;

public class Transacao : Entity
{
    public Transacao(Guid id, 
        string? codigoAutorizacao, 
        string? bandeiraCartao, 
        DateTime? dataTransacao,
        decimal valorTotal, 
        decimal custoTransacao, 
        StatusTransacao status, 
        string? tID, 
        string? nSU)
        : base(id)
    {
        CodigoAutorizacao = codigoAutorizacao;
        BandeiraCartao = bandeiraCartao;
        DataTransacao = dataTransacao;
        ValorTotal = valorTotal;
        CustoTransacao = custoTransacao;
        Status = status;
        TID = tID;
        NSU = nSU;
    }

    public string? CodigoAutorizacao { get; set; }
    public string? BandeiraCartao { get; set; }
    public DateTime? DataTransacao { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal CustoTransacao { get; set; }
    public StatusTransacao Status { get; set; }
    public string? TID { get; set; } // Id
    public string? NSU { get; set; } // Meio (paypal)

    public Guid PagamentoId { get; set; }

    // EF Relation
    public Pagamento Pagamento { get; set; }
}
