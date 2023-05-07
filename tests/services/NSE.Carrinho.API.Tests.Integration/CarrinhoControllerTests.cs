using FluentAssertions;
using NSE.Carrinho.API.Model;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace NSE.Carrinho.API.Tests.Integration;

public class CarrinhoControllerTests : IClassFixture<CarrinhoApiFactory>
{
    private readonly HttpClient _httpClient;

    public CarrinhoControllerTests(CarrinhoApiFactory apiFactory)
    {
        _httpClient = apiFactory.HttpClient;
    }

    [Fact]
    public async Task ObterCarrinho_RetornaUmCarrinhoVazio_QuandoClienteNaoTemCarrinhoIniciado()
    {   
        var response = await _httpClient.GetAsync("carrinho/");

        var responseContent = await response.Content.ReadFromJsonAsync<CarrinhoCliente>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Id.Should().Be(Guid.Empty);
        responseContent.ClienteId.Should().Be(Guid.Empty);
        responseContent.ValorTotal.Should().Be(0);
        responseContent.Desconto.Should().Be(0);
        responseContent.Itens.Should().BeEmpty();
    }
}