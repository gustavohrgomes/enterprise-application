using FluentAssertions;
using NSE.Carrinho.API.Model;
using System.Drawing;
using Xunit;

namespace NSE.Carrinho.API.Tests.Unit.Models;

public class CarrinhoClienteTests
{
    private readonly Guid _carrinhoClienteId;
    private readonly Guid _clienteId;

    public CarrinhoClienteTests()
    {
        _carrinhoClienteId = Guid.NewGuid();
        _clienteId = Guid.NewGuid();
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(3, 50, 150)]
    [InlineData(6, 24.9, 149.4)]
    [InlineData(2, 99.9, 199.8)]
    [InlineData(4, 233.4, 933.6)]
    public void CalcularValorCarrinho_DeveCalcularValorTotal_QuandoHouverProdutosNoCarrinho(int quantidade, decimal valor, decimal valorTotalEsperado)
    {
        // Arrange
        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
            Itens = new List<CarrinhoItem>
            {
                new CarrinhoItem
                {
                    CarrinhoId = _carrinhoClienteId,
                    Quantidade = quantidade,
                    Valor = valor
                }
            }
        };

        // Act
        sut.CalcularValorCarrinho();

        // Assert
        sut.ValorTotal.Should().Be(valorTotalEsperado);
        sut.ValorTotal.Should().BeGreaterThanOrEqualTo(0m);
    }

    [Fact]
    public void CalcularValorCarrinho_DeveCalcularValorComDesconto_QuandoForPassadoUmVoucherDePorcentagem()
    {
        // Arrange
        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
            Itens = new List<CarrinhoItem>
            {
                new CarrinhoItem
                {
                    CarrinhoId = _carrinhoClienteId,
                    Quantidade = 3,
                    Valor = 50
                }
            },
            VoucherUtilizado = true,
            Voucher = new()
            {
                Codigo = "10-OFF",
                Percentual = 10,
                TipoDesconto = TipoDescontoVoucher.Porcentagem
            }
        };

        // Act
        sut.CalcularValorCarrinho();

        // Assert
        sut.ValorTotal.Should().Be(135);
    }

    [Fact]
    public void CalcularValorCarrinho_DeveCalcularValorComDesconto_QuandoForPassadoUmVoucherDeValor()
    {
        // Arrange
        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
            Itens = new()
            {
                new CarrinhoItem
                {
                    CarrinhoId = _carrinhoClienteId,
                    Quantidade = 3,
                    Valor = 50
                }
            },
            VoucherUtilizado = true,
            Voucher = new()
            {
                Codigo = "35-OFF",
                ValorDesconto = 35,
                TipoDesconto = TipoDescontoVoucher.Valor
            }
        };

        // Act
        sut.CalcularValorCarrinho();

        // Assert
        sut.ValorTotal.Should().Be(115);
    }

    [Fact]
    public void CalcularValorCarrinho_DeveCalcularValorComDesconto_QuandoForPassadoUmVoucherENaoHouverProdutoNoCarrinho()
    {
        // Arrange
        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
            VoucherUtilizado = true,
            Voucher = new()
            {
                Codigo = "35-OFF",
                ValorDesconto = 35,
                TipoDesconto = TipoDescontoVoucher.Valor
            }
        };

        // Act
        sut.CalcularValorCarrinho();

        // Assert
        sut.ValorTotal.Should().Be(0);
    }

    [Fact]
    public void CalcularValorCarrinho_DeveZerarOValorTotalDaCompra_QuandoODescontoForMaiorQueOValorTotal()
    {
        // Arrange
        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
            Itens = new()
            {
                new CarrinhoItem
                {
                    Quantidade = 1,
                    Valor = 25
                }
            },
            VoucherUtilizado = true,
            Voucher = new()
            {
                Codigo = "35-OFF",
                ValorDesconto = 35,
                TipoDesconto = TipoDescontoVoucher.Valor
            }
        };

        // Act
        sut.CalcularValorCarrinho();

        // Assert
        sut.ValorTotal.Should().Be(0);
    }

    [Fact]
    public void AplicarVoucher_DeveAtribuirVoucherEMarcarComoUtilizado_QuandoUmVoucherValidoVouPassado()
    {
        // Arrange
        CarrinhoCliente sut = new();

        Voucher voucher = new()
        {
            Codigo = "10-OFF",
            Percentual = 10,
            TipoDesconto = TipoDescontoVoucher.Porcentagem
        };

        // Act
        sut.AplicarVoucher(voucher);

        // Assert
        sut.VoucherUtilizado.Should().BeTrue();
        sut.Voucher.Should().BeEquivalentTo(voucher);
    }

    [Fact]
    public void CarrinhoItemExistente_DeveRetornarTrue_QuandoHouverUmProdutoEquivalente()
    {
        // Arrange 
        var produtoId = Guid.NewGuid();
        
        CarrinhoItem item = new()
        {
            ProdutoId = produtoId
        };

        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
            Itens = new()
            {
                new CarrinhoItem
                {
                    ProdutoId = produtoId,
                    CarrinhoId = _carrinhoClienteId,
                }
            }
        };

        // Act
        var result = sut.CarrinhoItemExistente(item);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CarrinhoItemExistente_DeveRetornarFalse_QuandoNaoHouverUmProdutoEquivalente()
    {
        // Arrange 

        var produtoId = Guid.NewGuid();

        CarrinhoItem item = new()
        {
            ProdutoId = produtoId
        };

        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
            Itens = new()
            {
                new CarrinhoItem
                {
                    ProdutoId = Guid.NewGuid(),
                    CarrinhoId = _carrinhoClienteId,
                }
            }
        };

        // Act
        var result = sut.CarrinhoItemExistente(item);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ObterProdutoPorId_DeveRetornarProduto_QuandoHouverProdutoCorrespondente()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        var itemId = Guid.NewGuid();

        CarrinhoItem item = new()
        {
            Id = itemId,
            ProdutoId = produtoId
        };

        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
            Itens = new()
            {
                new CarrinhoItem
                {
                    Id = itemId,
                    ProdutoId = produtoId,
                }
            }
        };

        // Act
        var result = sut.ObterProdutoPorId(produtoId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(item);
    }

    [Fact]
    public void ObterProdutoPorId_DeveRetornarNull_QuandoNaoHouverProdutoCorrespondente()
    {
        // Arrange
        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
            Itens = new()
            {
                new CarrinhoItem
                {
                    Id = Guid.NewGuid(),
                    ProdutoId = Guid.NewGuid(),
                }
            }
        };

        // Act
        var result = sut.ObterProdutoPorId(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void AdicionarItem_DeveAdicionarUmItemNoCarrinho_QuandoEsseItemNaoExistir()
    {
        // Arrange
        var produtoId = Guid.NewGuid();

        CarrinhoItem item = new()
        {
            ProdutoId = produtoId,
            Quantidade = 1,
            Valor = 50,
            Nome = "Produto Teste"
        };

        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
        };

        // Act
        sut.AdicionarItem(item);

        // Assert
        sut.Itens.Should().NotBeNullOrEmpty();
        sut.Itens.Should().ContainEquivalentOf(item);
        sut.ValorTotal.Should().Be(50);
    }

    [Fact]
    public void AdicionarItem_DeveAtualizarUmItemNoCarrinho_QuandoEsseItemExistir()
    {
        // Arrange
        var produtoId = Guid.NewGuid();

        CarrinhoItem item = new()
        {
            ProdutoId = produtoId,
            Quantidade = 3,
            Valor = 50
        };

        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
            Itens = new()
            {
                new CarrinhoItem
                {
                    ProdutoId = produtoId,
                    Quantidade = 1,
                    Valor = 50
                }
            }
        };

        // Act
        sut.AdicionarItem(item);

        // Assert
        sut.Itens.Should().NotBeNullOrEmpty();
        sut.Itens.Single().Quantidade.Should().Be(4);
        sut.ValorTotal.Should().Be(200);
    }

    [Fact]
    public void AtualizarItem_DeveAtualizarUmItemNoCarrinho_QuandoEsseItemExistir()
    {
        // Arrange
        var produtoId = Guid.NewGuid();

        CarrinhoItem item = new()
        {
            ProdutoId = produtoId,
            Quantidade = 3,
            Valor = 50,
            Nome = "Produto Teste"
        };

        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
            Itens = new()
            {
                new CarrinhoItem
                {
                    ProdutoId = produtoId,
                    Quantidade = 1
                }
            }
        };

        // Act
        sut.AtualizarItem(item);

        // Assert
        sut.Itens.Should().NotBeNullOrEmpty();
        sut.Itens.Single().Quantidade.Should().Be(3);
        sut.ValorTotal.Should().Be(150);
    }

    [Fact]
    public void AtualizarUnidades_DeveAtualizarQuantidadeDeUmItem_QuandoPassadoUmaQuantidadeValida()
    {
        // Arrange
        var produtoId = Guid.NewGuid();

        CarrinhoItem item = new()
        {
            ProdutoId = produtoId,
            Quantidade = 1,
            Valor = 50,
            Nome = "Produto Teste"
        };

        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
        };

        sut.AdicionarItem(item);

        // Act
        sut.AtualizarUnidades(item, 3);

        // Assert
        sut.Itens.Should()
            .NotBeNullOrEmpty()
            .And.HaveCount(1);
        sut.Itens.Single().Quantidade.Should().Be(3);
        sut.ValorTotal.Should().Be(150);
    }

    [Fact]
    public void RemoverItem_DeveRemoverUmItemDoCarrinho_QuandoHouverItemCorrespondente()
    {
        // Arrange
        var produtoId = Guid.NewGuid();

        CarrinhoItem item = new()
        {
            ProdutoId = produtoId,
            Quantidade = 1,
            Valor = 50,
            Nome = "Produto Teste"
        };

        CarrinhoCliente sut = new()
        {
            Id = _carrinhoClienteId,
            ClienteId = _clienteId,
        };

        sut.AdicionarItem(item);

        // Act
        sut.RemoverItem(item);

        // Assert
        sut.ValorTotal.Should().Be(0);
        sut.Itens
            .Should().BeNullOrEmpty()
            .And.HaveCount(0);
    }
}
