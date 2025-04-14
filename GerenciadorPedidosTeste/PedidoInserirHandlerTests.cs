using FluentAssertions;
using NSubstitute;
using Bogus;
using AutoMapper;
using Microsoft.Extensions.Logging;

using GerenciadorPedidos.Api.Handlers;
using GerenciadorPedidos.Domain.Commands;
using GerenciadorPedidos.Domain.Enum;
using GerenciadorPedidos.Domain.Model;
using GerenciadorPedidos.Infra.Repositorio.Interface;

public class PedidoInserirHandlerTests
{
    private readonly IPedidoRepositorio _pedidoRepositorio = Substitute.For<IPedidoRepositorio>();
    private readonly ILogger<PedidoInserirHandler> _logger = Substitute.For<ILogger<PedidoInserirHandler>>();
    private readonly IMapper _mapper;

    private readonly PedidoInserirHandler _handler;
    private readonly Faker _faker = new();

    public PedidoInserirHandlerTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Pedido, Pedido>().ReverseMap(); // ou seu perfil real
        });

        _mapper = config.CreateMapper();

        _handler = new PedidoInserirHandler(_pedidoRepositorio, _logger, _mapper);
    }

    [Fact]
    public async Task Deve_Inserir_Pedido_Com_Sucesso()
    {
        // Arrange
        var command = GerarPedidoInserirCommand();

        _pedidoRepositorio.ObterPorId(command.Pedido.PedidoId).Returns((Pedido)null);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(EnumStatusPedido.Criado);
        result.Id.Should().Be(command.Id);

        await _pedidoRepositorio.Received(1).Inserir(Arg.Any<Pedido>());
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Se_Command_For_Nulo()
    {
        Func<Task> act = async () => await _handler.Handle(null);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("command");
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Se_PedidoId_For_Invalido()
    {
        var command = GerarPedidoInserirCommand();
        command.Pedido.PedidoId = 0;

        Func<Task> act = async () => await _handler.Handle(command);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("command.PedidoId");
    }

    // outros testes semelhantes para ClienteId inválido, lista de Itens vazia, etc...

    #region Helpers

    private PedidoInserirCommand GerarPedidoInserirCommand()
    {
        var pedidoId = _faker.Random.Int(1, 1000);

        var itens = new List<Item>
        {
            new Item
            {
                ProdutoId = _faker.Random.Int(1, 100),
                Quantidade = _faker.Random.Int(1, 10),
                Valor = _faker.Random.Decimal(10, 100)
            }
        };

        var pedido = new Pedido
        {
            PedidoId = pedidoId,
            ClienteId = _faker.Random.Int(1, 1000),
            ValorTotal = 150.00m,
            Imposto = 10.00m,
            Itens = itens,
            Status = EnumStatusPedido.Criado
        };

        return new PedidoInserirCommand
        {
            Oid = _faker.Random.Guid().ToString(),
            Id = _faker.Random.Guid().ToString(),
            Sid = _faker.Random.Guid().ToString(),
            Pedido = pedido
        };
    }

    #endregion
}
