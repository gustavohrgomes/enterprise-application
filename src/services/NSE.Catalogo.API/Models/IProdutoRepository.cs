﻿using NSE.Core.Data;

namespace NSE.Catalogo.API.Models;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>> ObterTodos();
    Task<PagedResult<Produto>> ObterTodosPaginados(PaginationFilter pagedFilter);
    Task<Produto> ObterPorId(Guid id);
    void Adicionar(Produto produto);
    void Atualizar(Produto produto);
    Task<IList<Produto>> ObterProdutosPorId(string produtosIds);
}
