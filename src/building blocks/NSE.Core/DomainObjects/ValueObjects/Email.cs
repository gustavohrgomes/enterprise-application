﻿using System.Text.RegularExpressions;

namespace NSE.Core.DomainObjects.ValueObjects;

public class Email
{
    public const int EnderecoMaxLength = 254;
    public const int EnderecoMinLength = 5;

    private Email(string endereco)
    {
        Endereco = endereco;
    }

    protected Email() { }

    public string Endereco { get; private set; }
    
    public static Email Create(string endereco)
    {
        if (!Validar(endereco)) throw new DomainException("E-mail inválido");
        return new Email(endereco);
    }

    public static bool Validar(string email)
    {
        var regexEmail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        return regexEmail.IsMatch(email);
    }
}
