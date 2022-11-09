using NSE.Core.Data;

namespace NSE.Pagamentos.API.Models;

public interface IPagamentoRepository : IRepository<Pagamento>
{
    void AdicionarPagamento(Pagamento pagamento);
    void AdicionarTransacao(Transacao transacao);
    Task<IEnumerable<Transacao>> ObterTransacoesPorPedidoId(Guid pedidoId);
}
