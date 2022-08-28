using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Models;
using NSE.Core.Data;

namespace NSE.Catalogo.API.Data.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly CatalogContext _context;
    
    public ProdutoRepository(CatalogContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<IEnumerable<Produto>> ObterTodos() 
        => await _context.Produtos.AsNoTracking().ToListAsync();

    public async Task<Produto> ObterPorId(Guid id) 
        => await _context.Produtos.FindAsync(id);

    public void Adicionar(Produto produto) 
        => _context.Produtos.Add(produto);

    public void Atualizar(Produto produto) 
        => _context.Produtos.Update(produto);

    public async Task<IList<Produto>> ObterProdutosPorId(string produtosIds)
    {
        var splitedProdutosIds = produtosIds
                    .Split(",")
                    .Select(id => (Ok: Guid.TryParse(id, out var x), Value: x));

        if (!splitedProdutosIds.All(id => id.Ok)) return new List<Produto>();

        var idsValue = splitedProdutosIds.Select(id => id.Value);

        return await _context.Produtos
            .AsNoTracking()
            .Where(p => idsValue.Contains(p.Id) && p.Ativo)
            .ToListAsync();
    }

    #region Disposable members

    private bool disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context?.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion Disposable members
}
