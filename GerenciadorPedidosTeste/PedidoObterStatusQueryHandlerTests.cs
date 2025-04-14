using FluentAssertions;
using NSubstitute;
using Bogus;
using Microsoft.Extensions.Logging;

using GerenciadorPedidos.Api.Handlers;
using GerenciadorPedidos.Domain.Query;
using GerenciadorPedidos.Domain.Enum;
using GerenciadorPedidos.Domain.Model;
using GerenciadorPedidos.Infra.Repositorio.Interface;

public class PedidoObterStatusQueryHandlerTests
{
    private readonly IPedidoRepositorio _pedidoRepositorio = Substitute.For<IPedidoRepositorio>();
    private readonly ILogger<PedidoObterQueryHandler> _logger = Substitute.For<ILogger<PedidoObterQueryHandler>>();
    private readonly PedidoObterStatusQueryHandler _handler;
    private readonly Faker _faker = new();

    public PedidoObterStatusQueryHandlerTests()
    {
        _handler = new PedidoObterStatusQueryHandler(_pedidoRepositorio, _logger);
    }

    [Fact]
    public async Task Deve_Obter_Pedidos_Por_Status_Com_Sucesso()
    {
        // Arrange
        var query = GerarPedidoObterStatusQuery();

        var pedidosFake = new List<Pedido>
        {
            new Pedido
            {
                PedidoId = _faker.Random.Int(1, 999),
                ClienteId = _faker.Random.Int(1, 999),
                ValorTotal = _faker.Random.Decimal(100, 1000),
                Imposto = _faker.Random.Decimal(10, 100),
                Itens = new(),
                Status = query.status
            }
        };

        _pedidoRepositorio.ObterPorStatus(query.status).Returns(pedidosFake);

        // Act
        var result = await _handler.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(query.Id);
        result.Pedidos.Should().HaveCountGreaterThan(0);
        result.Pedidos[0].Status.Should().Be(query.status);
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Se_Query_For_Nula()
    {
        Func<Task> act = async () => await _handler.Handle(null);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Consulta sem parâmetro informado");
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Se_TenantId_For_Nulo()
    {
        var query = GerarPedidoObterStatusQuery();
        query.Id = null;

        Func<Task> act = async () => await _handler.Handle(query);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Informe o Tenance Id");
    }

    #region Helpers

    private PedidoObterStatusQuery GerarPedidoObterStatusQuery()
    {
        return new PedidoObterStatusQuery
        {
            Id = _faker.Random.Guid().ToString(),
            status = _faker.PickRandom<EnumStatusPedido>()
        };
    }

    #endregion
}
