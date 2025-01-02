using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSE.Core.Communication;
using NSE.WebAPI.Core.Extensions;
using NSE.WebAPI.Core.HttpResponses;

namespace NSE.WebAPI.Core.Controllers;

[ApiController]
public abstract class MainController : ControllerBase
{
    protected ICollection<string> Errors = new List<string>();

    protected ActionResult CustomResponse(object result = null)
    {
        if (OperacaoValida()) return Ok(result);

        return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            { "Mensagens", Errors.ToArray() }
        }));
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);

        foreach (var erro in erros)
            AdicionarErroProcessamento(erro.ErrorMessage);

        return CustomResponse();
    }

    protected ActionResult CustomResponse(ValidationResult validationResult)
    {
        foreach (var erro in validationResult.Errors)
            AdicionarErroProcessamento(erro.ErrorMessage);

        return CustomResponse();
    }

    protected ActionResult CustomResponse(ResponseResult resposta)
    {
        ResponsePossuiErros(resposta);

        return CustomResponse();
    }

    protected void AdicionarErroProcessamento(string erro) => Errors.Add(erro);

    protected void LimparErrosProcessamento() => Errors.Clear();

    protected bool OperacaoValida() => !Errors.Any();

    protected bool ResponsePossuiErros(ResponseResult resposta)
    {
        if (resposta == null || !resposta.Errors.Any()) return false;

        foreach (var mensagem in resposta.Errors)
        {
            AdicionarErroProcessamento(mensagem);
        }

        return true;
    }

    #region NonAction Methods

    [NonAction]
    protected OkObjectResult HttpOk()
    {
        return Ok(new HttpOkResponse());
    }

    [NonAction]
    protected OkObjectResult HttpOk<T>(T value)
    {
        return Ok(new HttpOkResponse<T>(value));
    }

    [NonAction]
    protected CreatedAtRouteResult HttpCreatedAtRoute(string routeName, object routeValues)
    {
        return CreatedAtRoute(routeName, routeValues, new HttpCreatedResponse());
    }

    [NonAction]
    protected CreatedAtRouteResult HttpCreatedAtRoute<T>(string routeName, object routeValues, T value)
    {
        return CreatedAtRoute(routeName, routeValues, new HttpCreatedResponse<T>(value));
    }

    [NonAction]
    protected ObjectResult HttpCreated()
    {
        var createdObjectResult = new ObjectResult(new HttpCreatedResponse())
        {
            StatusCode = StatusCodes.Status201Created
        };

        return createdObjectResult;
    }

    [NonAction]
    protected ObjectResult HttpCreated<T>(T value)
    {
        var createdObjectResult = new ObjectResult(new HttpCreatedResponse<T>(value))
        {
            StatusCode = StatusCodes.Status201Created
        };

        return createdObjectResult;
    }

    [NonAction]
    protected NoContentResult HttpNoContent()
    {
        return NoContent();
    }

    [NonAction]
    protected BadRequestObjectResult HttpBadRequest()
    {
        return BadRequest(new HttpBadRequestResponse(Errors.ToArray()));
    }

    [NonAction]
    protected BadRequestObjectResult HttpBadRequest(params string[] errors)
    {
        return BadRequest(new HttpBadRequestResponse(errors));
    }

    [NonAction]
    protected BadRequestObjectResult HttpBadRequest(ModelStateDictionary modelState)
    {
        var errors = modelState is not null && !modelState.IsValid ? modelState.GetErrors() : Array.Empty<string>();

        return BadRequest(new HttpBadRequestResponse(errors));
    }

    [NonAction]
    protected BadRequestObjectResult HttpBadRequest(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
            AdicionarErroProcessamento(error.ErrorMessage);

        return BadRequest(new HttpBadRequestResponse(Errors.ToArray()));
    }

    [NonAction]
    protected ObjectResult HttpUnauthorized(params string[] errors)
    {
        var unauthorizedObjectResult = new ObjectResult(new HttpUnauthorizedAccessResponse(errors))
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };

        return unauthorizedObjectResult;
    }

    [NonAction]
    protected ObjectResult HttpForbid()
    {
        var forbiddenObjectResult = new ObjectResult(new HttpForbiddenResponse())
        {
            StatusCode = StatusCodes.Status403Forbidden
        };

        return forbiddenObjectResult;
    }

    [NonAction]
    protected NotFoundObjectResult HttpNotFound()
    {
        return NotFound(new HttpNotFoundResponse(Errors.ToArray()));
    }

    [NonAction]
    protected NotFoundObjectResult HttpNotFound(params string[] errors)
    {
        return NotFound(new HttpNotFoundResponse(errors));
    }

    [NonAction]
    protected ConflictObjectResult HttpConflict(params string[] errors)
    {
        return Conflict(new HttpConflictResponse(errors));
    }

    [NonAction]
    protected ConflictObjectResult HttpConflict(ModelStateDictionary modelState)
    {
        var errors = modelState != null && !modelState.IsValid ? modelState.GetErrors() : Array.Empty<string>();

        return Conflict(new HttpConflictResponse(errors));
    }

    [NonAction]
    protected ObjectResult HttpInternalServerError(params string[] errors)
    {
        var internalServerErrorObjectResult = new ObjectResult(new HttpInternalServerErrorResponse(errors))
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        return internalServerErrorObjectResult;
    }

    #endregion NonAction Methods
}

