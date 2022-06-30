using NSE.WebApp.MVC.Models;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace NSE.WebApp.MVC.Services;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly HttpClient _httpClient;

    public AutenticacaoService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
    {
        var loginContent = new StringContent(JsonSerializer.Serialize(usuarioLogin), Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = await _httpClient.PostAsync("http://localhost:5267/api/identidade/login", loginContent);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStreamAsync(), options);
    }

    public async Task<UsuarioRespostaLogin> Registrar(UsuarioRegistro usuarioRegistro)
    {
        var registroContent = new StringContent(JsonSerializer.Serialize(usuarioRegistro), Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = await _httpClient.PostAsync("http://localhost:5267/api/identidade/nova-conta", registroContent);

        return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStreamAsync());
    }
}
