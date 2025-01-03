﻿using Microsoft.EntityFrameworkCore;
using NSE.Pedidos.Domain.Vouchers;

namespace NSE.Pedidos.Infra.Data.Repository;

public class VoucherRepository : IVoucherRepository
{
    private readonly PedidosContext _context;

    public VoucherRepository(PedidosContext context)
    {
        _context = context;
    }

    public async Task<Voucher> ObterVoucherPorCodigo(string codigo) 
        => await _context.Vouchers.FirstOrDefaultAsync(p => p.Codigo == codigo);

    public void Atualizar(Voucher voucher) => _context.Vouchers.Update(voucher);

    public void Dispose() => _context.Dispose();
}