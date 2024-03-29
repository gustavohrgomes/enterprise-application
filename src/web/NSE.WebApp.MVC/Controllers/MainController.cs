﻿using Microsoft.AspNetCore.Mvc;
using NSE.Core.Communication;

namespace NSE.WebApp.MVC.Controllers;

public abstract class MainController : Controller
{
    protected bool ResponsePossuiErros(ResponseResult response)
    {
        if (response != null && response.Errors.Any())
        {
            foreach (var mensagem in response.Errors)
                ModelState.AddModelError(string.Empty, mensagem);

            return true;
        }

        return false;
    }

    protected void AdicionarErroValidacao(string mensagem) => ModelState.AddModelError(string.Empty, mensagem);

    protected bool OperacaoValida() => ModelState.ErrorCount == 0;
}
