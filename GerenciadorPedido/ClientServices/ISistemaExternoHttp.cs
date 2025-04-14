using GerenciadorPedidos.Domain.Commands;

namespace GerenciadorPedidos.Api.ClientServices
{
    public interface ISistemaExternoHttp
    {
        Task<PedidoInserirCommandResult> PedidoEnviar(PedidoInserirCommand command);
    }
}
