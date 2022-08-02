using NSE.WebApp.MVC.Exceptions;
using NSE.WebApp.MVC.Models;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace NSE.WebApp.MVC.Services;

public abstract class Service
{
    protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage response)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStreamAsync(), options);
    }

    protected static StringContent ObterConteudo(object objeto)
        => new(JsonSerializer.Serialize(objeto), Encoding.UTF8, MediaTypeNames.Application.Json);

    protected static bool TratarErrosResponse(HttpResponseMessage response)
    {
        switch ((int)response.StatusCode)
        {
            case 400:
                return false;
            case 401:
            case 403:
            case 404:
            case 500:
                throw new CustomHttpRequestException(response.StatusCode);
        }

        response.EnsureSuccessStatusCode();
        return true;
    }

    protected ResponseResult ReturnOk() => new();
}
