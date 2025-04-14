using GerenciadorPedidos.Domain.Query;
using GerenciadorPedidos.Infra.Repositorio.Interface;
using iHUB.Domain.Cqrs.Handlers;

namespace GerenciadorPedidos.Api.Handlers
{
    public class PedidoObterQueryHandler : IQueryHandler<PedidoObterQuery, PedidoObterQueryResult>
    {
        public readonly IPedidoRepositorio _pedidoRepositorio;
        public readonly ILogger<PedidoObterQueryHandler> _logger;

        public PedidoObterQueryHandler(IPedidoRepositorio pedidoRepositorio, ILogger<PedidoObterQueryHandler> logger)
        {
            _pedidoRepositorio = pedidoRepositorio;
            _logger = logger;
        }

        public async Task<PedidoObterQueryResult> Handle(PedidoObterQuery query)
        {

            if (query == null)
                throw new ArgumentException("command");
            if (query.PedidoId <= 0)
                throw new ArgumentException("Informe o número do pedido");
            if (string.IsNullOrEmpty(query.Id))
                throw new ArgumentException("Informe o Tenance Id");

            var retorno = await _pedidoRepositorio.ObterPorId(query.PedidoId);

            var queryResult = new PedidoObterQueryResult()
            {
                Id = query.Id,
                Pedido = retorno
            };
            
            _logger.LogInformation($"PEDIDO_OBTER| Consulta finalizada com sucesso por pedido {query.PedidoId} - Id: {query.Id}");

            return queryResult;
        }
    }
}
