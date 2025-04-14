using GerenciadorPedidos.Domain.Query;
using GerenciadorPedidos.Infra.Repositorio.Interface;
using iHUB.Domain.Cqrs.Handlers;

namespace GerenciadorPedidos.Api.Handlers
{
    public class PedidoObterStatusQueryHandler : IQueryHandler<PedidoObterStatusQuery, PedidoObterStatusQueryResult>
    {

        public readonly IPedidoRepositorio _pedidoRepositorio;
        public readonly ILogger<PedidoObterQueryHandler> _logger;

        public PedidoObterStatusQueryHandler(IPedidoRepositorio pedidoRepositorio, ILogger<PedidoObterQueryHandler> logger)
        {
            _pedidoRepositorio = pedidoRepositorio;
            _logger = logger;
        }

        public async Task<PedidoObterStatusQueryResult> Handle(PedidoObterStatusQuery query)
        {

            if (query == null)
                throw new ArgumentException("Consulta sem parâmetro informado");
            if (string.IsNullOrEmpty(query.Id))
                throw new ArgumentException("Informe o Tenance Id");

            var retorno = await _pedidoRepositorio.ObterPorStatus(query.status);

            var queryResult = new PedidoObterStatusQueryResult()
            {
                Id = query.Id,
                Pedidos = retorno
            };

            _logger.LogInformation($"PEDIDO_OBTER_STATUS| Consulta finalizada com sucesso do status {query.status} - Id: {query.Id}");

            return queryResult;
        }
    }
}
