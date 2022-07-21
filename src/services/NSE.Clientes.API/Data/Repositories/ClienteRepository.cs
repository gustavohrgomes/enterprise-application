using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Models;
using NSE.Core.Data;

namespace NSE.Clientes.API.Data.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ClientesContext _context;

    public ClienteRepository(ClientesContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IUnitOfWork UnitOfWork => _context;

    public void Adicionar(Cliente cliente) => _context.Clientes.Add(cliente);

    public async Task<Cliente> ObterPorCpf(string cpf) 
        => await _context.Clientes.Where(c => c.Cpf.Numero == cpf).FirstOrDefaultAsync();

    public async Task<IEnumerable<Cliente>> ObterTodos()
        => await _context.Clientes.AsNoTrackingWithIdentityResolution().ToListAsync();

    #region Disposable members

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

    #endregion
}
