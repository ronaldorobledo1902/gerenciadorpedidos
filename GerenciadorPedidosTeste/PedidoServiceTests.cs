using FluentAssertions;
using Bogus;
using NSubstitute;
using Microsoft.Extensions.Configuration;

using GerenciadorPedidos.Api.Service;
using GerenciadorPedidos.Domain.Model;

public class PedidoServiceTests
{
    private readonly Faker _faker = new();

    [Fact]
    public async Task Deve_Calcular_Imposto_Com_ImpostoAtual()
    {
        // Arrange
        var pedido = GerarPedidoFake();

        var config = Substitute.For<IConfiguration>();
        config["NovoImposto"].Returns("ImpostoAtual");

        var service = new PedidoService(config);

        // Act
        var result = await service.CalcularImposto(pedido);

        // Assert
        result.ValorTotal.Should().Be(pedido.Itens.Sum(i => i.Quantidade * i.Valor));
        result.Imposto.Should().Be(result.ValorTotal * 0.3m);
    }

    [Fact]
    public async Task Deve_Calcular_Imposto_Com_ImpostoAntigo()
    {
        // Arrange
        var pedido = GerarPedidoFake();

        var config = Substitute.For<IConfiguration>();
        config["NovoImposto"].Returns("ImpostoAntigo");

        var service = new PedidoService(config);

        // Act
        var result = await service.CalcularImposto(pedido);

        // Assert
        result.ValorTotal.Should().Be(pedido.Itens.Sum(i => i.Quantidade * i.Valor));
        result.Imposto.Should().Be(result.ValorTotal * 0.2m);
    }

    #region Helpers

    private Pedido GerarPedidoFake()
    {
        return new Pedido
        {
            PedidoId = _faker.Random.Int(1, 9999),
            ClienteId = _faker.Random.Int(1, 9999),
            Itens = new List<Item>
            {
                new Item
                {
                    ProdutoId = 1,
                    Quantidade = 2,
                    Valor = 100m
                },
                new Item
                {
                    ProdutoId = 2,
                    Quantidade = 1,
                    Valor = 50m
                }
            }
        };
    }

    #endregion
}
