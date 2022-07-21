﻿using NSE.Core.DomainObjects;
using NSE.Core.DomainObjects.ValueObjects;

namespace NSE.Clientes.API.Models;

public class Cliente : Entity, IAggregateRoot
{
    public Cliente(Guid id, string nome, string email, string cpf)
    {
        Id = id;
        Nome = nome;
        Email = new Email(email);
        Cpf = new Cpf(cpf);
        Excluido = false;
    }

    // EF Relation
    protected Cliente() { }

    public string Nome { get; private set; }
    public Email Email { get; private set; }
    public Cpf Cpf { get; private set; }
    public bool Excluido { get; private set; }
    public Endereco Endereco { get; private set; }

    public void AlterarEmail(string email) => Email = new Email(email);

    public void AtribuirEndereço(Endereco endereco) => Endereco = endereco;
}
