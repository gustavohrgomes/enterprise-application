﻿using NSE.Core.DomainObjects;

namespace NSE.Catalogo.API.Models;

public class Produto : AggregateRoot
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public bool Ativo { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataCadastro { get; set; }
    public string Imagem { get; set; }
    public int QuantidadeEstoque { get; set; }

    public void RetirarEstoque(int quantidade)
    {
        if (QuantidadeEstoque >= quantidade)
            QuantidadeEstoque -= quantidade;
    }

    public bool EstaDisponivel(int quantidade) => Ativo && QuantidadeEstoque >= quantidade;
}
