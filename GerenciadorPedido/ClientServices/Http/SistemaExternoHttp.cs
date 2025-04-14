using GerenciadorPedidos.Domain.Commands;

namespace GerenciadorPedidos.Api.ClientServices.Http
{
    public class SistemaExternoHttp : BaseHttp<SistemaExternoHttp>, ISistemaExternoHttp
    {

        public SistemaExternoHttp(HttpClient client, ILogger<SistemaExternoHttp> log) : base(client, log) { }

        public Task<PedidoInserirCommandResult> PedidoEnviar(PedidoInserirCommand command)
         => PostAsync<PedidoInserirCommand, PedidoInserirCommandResult>("PEDIDO_RECEBER", "v1/pedido/receber", command);

    }
}
