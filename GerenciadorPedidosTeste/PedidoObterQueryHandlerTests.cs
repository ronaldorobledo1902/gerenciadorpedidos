using Xunit;
using FluentAssertions;
using NSubstitute;
using Bogus;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

using GerenciadorPedidos.Api.Handlers;
using GerenciadorPedidos.Domain.Query;
using GerenciadorPedidos.Domain.Model;
using GerenciadorPedidos.Infra.Repositorio.Interface;
using GerenciadorPedidos.Domain.Enum;

public class PedidoObterQueryHandlerTests
{
    private readonly IPedidoRepositorio _pedidoRepositorio = Substitute.For<IPedidoRepositorio>();
    private readonly ILogger<PedidoObterQueryHandler> _logger = Substitute.For<ILogger<PedidoObterQueryHandler>>();
    private readonly PedidoObterQueryHandler _handler;
    private readonly Faker _faker = new();

    public PedidoObterQueryHandlerTests()
    {
        _handler = new PedidoObterQueryHandler(_pedidoRepositorio, _logger);
    }

    [Fact]
    public async Task Deve_Obter_Pedido_Com_Sucesso()
    {
        // Arrange
        var query = GerarPedidoObterQuery();

        var pedidoFake = new Pedido
        {
            PedidoId = query.PedidoId,
            ClienteId = _faker.Random.Int(1, 999),
            ValorTotal = _faker.Random.Decimal(100, 1000),
            Imposto = _faker.Random.Decimal(10, 100),
            Itens = new(),
            Status = EnumStatusPedido.Criado
        };

        _pedidoRepositorio.ObterPorId(query.PedidoId).Returns(pedidoFake);

        // Act
        var result = await _handler.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(query.Id);
        result.Pedido.Should().NotBeNull();
        result.Pedido.PedidoId.Should().Be(query.PedidoId);
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Se_Query_For_Nula()
    {
        Func<Task> act = async () => await _handler.Handle(null);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("command");
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Se_PedidoId_For_Invalido()
    {
        var query = GerarPedidoObterQuery();
        query.PedidoId = 0;

        Func<Task> act = async () => await _handler.Handle(query);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Informe o número do pedido");
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Se_TenantId_For_Nulo()
    {
        var query = GerarPedidoObterQuery();
        query.Id = null;

        Func<Task> act = async () => await _handler.Handle(query);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Informe o Tenance Id");
    }

    #region Helpers

    private PedidoObterQuery GerarPedidoObterQuery()
    {
        return new PedidoObterQuery
        {
            Id = _faker.Random.Guid().ToString(),
            PedidoId = _faker.Random.Int(1, 9999)
        };
    }

    #endregion
}
