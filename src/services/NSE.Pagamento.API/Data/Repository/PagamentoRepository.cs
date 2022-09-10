using NSE.Core.Data;
using NSE.Pagamentos.API.Models;

namespace NSE.Pagamentos.API.Data.Repository;

public class PagamentoRepository : IPagamentoRepository
{
    private readonly PagamentosContext _context;

    public PagamentoRepository(PagamentosContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public void AdicionarPagamento(Pagamento pagamento)
        => _context.Pagamentos.Add(pagamento);

    #region Disposable Members

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    # endregion Disposable Members
}
