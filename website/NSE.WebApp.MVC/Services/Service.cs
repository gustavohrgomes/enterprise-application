using NSE.Core.Communication;
using NSE.WebApp.MVC.Exceptions;
using System.Net;
using System.Net.Mime;
using System.Security.Authentication;
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

        var deserializedResponse = DeJson<T>(await response.Content.ReadAsStringAsync(), options);

        return deserializedResponse;
    }

    protected T DeJson<T>(string json, JsonSerializerOptions options)
    {
        var objectResult = (T)DeJson(json, typeof(T), options);

        return objectResult;
    }

    protected object DeJson(string json, Type type, JsonSerializerOptions options)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;

        var objectResult = JsonSerializer.Deserialize(json, type, options);

        return objectResult;
    }

    protected static StringContent ParaConteudoHttp(object objeto)
        => new(JsonSerializer.Serialize(objeto), Encoding.UTF8, MediaTypeNames.Application.Json);

    protected static async Task<bool> TratarResponseAsync(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.BadRequest) return false;

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new AuthenticationException(content);
            }

            throw new CustomHttpRequestException(response.StatusCode, content);
        }

        response.EnsureSuccessStatusCode();
        return true;
    }

    protected ResponseResult Ok() => new();
}
