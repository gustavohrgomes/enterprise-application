using System;
using System.Collections.Generic;
using System.Linq;
using NSE.Core.DomainObjects;
using NSE.Pedidos.Domain.Vouchers;

namespace NSE.Pedidos.Domain.Pedidos;

public class Pedido : Entity, IAggregateRoot
{
    public Pedido(
        Guid clienteId, 
        decimal valorTotal, 
        List<PedidoItem> pedidoItems, 
        bool voucherUtilizado = false, 
        decimal desconto = 0, 
        Guid? voucherId = null)
    {
        ClienteId = clienteId;
        ValorTotal = valorTotal;
        _pedidoItems = pedidoItems;

        Desconto = desconto;
        VoucherUtilizado = voucherUtilizado;
        VoucherId = voucherId;
    }

    // EF ctor
    protected Pedido() { }

    public int Codigo { get; private set; }
    public Guid ClienteId { get; private set; }
    public Guid? VoucherId { get; private set; }
    public bool VoucherUtilizado { get; private set; }
    public decimal Desconto { get; private set; }
    public decimal ValorTotal { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public PedidoStatus PedidoStatus { get; private set; }

    private readonly List<PedidoItem> _pedidoItems;
    public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;
    
    public Endereco Endereco { get; private set; }

    // EF Rel.
    public Voucher Voucher { get; private set; }

    public void AutorizarPedido() => PedidoStatus = PedidoStatus.Autorizado;

    public void AtribuirVoucher(Voucher voucher)
    {
        VoucherUtilizado = true;
        VoucherId = voucher.Id;
        Voucher = voucher;
    }

    public void AtribuirEndereco(Endereco endereco) => Endereco = endereco;

    public void CalcularValorPedido()
    {
        ValorTotal = PedidoItems.Sum(p => p.CalcularValor());
        CalcularValorTotalDesconto();
    }

    public void CalcularValorTotalDesconto()
    {
        if (!VoucherUtilizado) return;

        var desconto = Voucher.TipoDesconto switch
        {
            TipoDescontoVoucher.Porcentagem => CalcularValorDescontoPorcentagem(),
            TipoDescontoVoucher.Valor => Voucher.ValorDesconto.HasValue ? Voucher.ValorDesconto.Value : 0,
            _ => 0
        };

        var valor = ValorTotal -= desconto;

        ValorTotal = valor < 0 ? 0 : valor;
        Desconto = desconto;
    }

    private decimal CalcularValorDescontoPorcentagem()
    {
        if (!Voucher.Percentual.HasValue) return 0;

        return ValorTotal * Voucher.Percentual.Value / 100;
    }
    
}