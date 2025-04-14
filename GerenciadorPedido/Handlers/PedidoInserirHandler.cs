using AutoMapper;
using GerenciadorPedidos.Domain.Commands;
using GerenciadorPedidos.Domain.Enum;
using GerenciadorPedidos.Domain.Model;
using GerenciadorPedidos.Infra.Repositorio.Interface;
using iHUB.Domain.Cqrs.Handlers;

namespace GerenciadorPedidos.Api.Handlers
{

    public class PedidoInserirHandler : ICommandHandler<PedidoInserirCommand, PedidoInserirCommandResult>
    {

        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly ILogger<PedidoInserirHandler> _logger;
        private readonly IMapper _mapper;

        public PedidoInserirHandler(IPedidoRepositorio pedidoRepositorio, ILogger<PedidoInserirHandler> logger, IMapper mapper)
        {
            _pedidoRepositorio = pedidoRepositorio;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PedidoInserirCommandResult> Handle(PedidoInserirCommand command)
        {
            if (command == null)
                throw new ArgumentException("command");
            if (command.Pedido.PedidoId <= 0)
                throw new ArgumentException("command.PedidoId");
            if (command.Pedido.ClienteId <= 0)
                throw new ArgumentException("Cliente inválido ou inexistente");
            if (command.Pedido.Itens.Count <= 0)
                throw new ArgumentException("Pedido sem itens informado");
            if (string.IsNullOrEmpty(command.Id))
                throw new ArgumentException("Informe o Tenance Id");


            command.Pedido.Itens.ForEach(f =>
            {
                if (f.Valor <= 0)
                    throw new ArgumentException("Existe item no pedido sem valor informado");
                if (f.Quantidade <= 0)
                    throw new ArgumentException("Existe item no pedido sem quantidade informada");
                if (f.ProdutoId <= 0)
                    throw new ArgumentException("Existe item com produlo inválido");
            });


            var retorno  = await _pedidoRepositorio.ObterPorId(command.Pedido.PedidoId);

            if (retorno != null)
                throw new ArgumentException("Pedido já registrado no sistema");

            var pedido = _mapper.Map<Pedido>(command.Pedido);

            pedido.Status = EnumStatusPedido.Criado;
            await _pedidoRepositorio.Inserir(pedido);

            var result = new PedidoInserirCommandResult()
            {
                Id = command.Id,
                Status = EnumStatusPedido.Criado,
            };

            _logger.LogInformation($"PEDIDO_INSERIR| Pedido criado com sucesso - Id: {command.Id}");

            return result;
        }
    }
}
