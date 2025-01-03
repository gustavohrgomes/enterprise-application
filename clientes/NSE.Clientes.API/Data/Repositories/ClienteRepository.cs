﻿using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Models;

namespace NSE.Clientes.API.Data.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ClientesContext _context;

    public ClienteRepository(ClientesContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Adicionar(Cliente cliente) => _context.Clientes.Add(cliente);

    public async Task<Cliente> ObterPorCpf(string cpf) 
        => await _context.Clientes.Where(c => c.Cpf.Numero == cpf).FirstOrDefaultAsync();

    public async Task<IEnumerable<Cliente>> ObterTodos()
        => await _context.Clientes.AsNoTrackingWithIdentityResolution().ToListAsync();

    public async Task<Endereco> ObterEnderecoPorId(Guid id)
        => await _context.Enderecos.Where(e => e.ClienteId == id).FirstOrDefaultAsync();

    public void AdicionarEndereco(Endereco endereco) => _context.Enderecos.Add(endereco);

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
