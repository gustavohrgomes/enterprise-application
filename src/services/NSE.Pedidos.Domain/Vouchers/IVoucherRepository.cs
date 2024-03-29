﻿using System.Threading.Tasks;
using NSE.Core.Data;

namespace NSE.Pedidos.Domain.Vouchers;

public interface IVoucherRepository
{
    Task<Voucher> ObterVoucherPorCodigo(string codigo);
    void Atualizar(Voucher voucher);
}