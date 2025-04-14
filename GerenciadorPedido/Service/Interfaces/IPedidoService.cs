using GerenciadorPedidos.Domain.Model;

namespace GerenciadorPedidos.Api.Service.Interfaces
{
    public interface IPedidoService
    {
        Task<Pedido> CalcularImposto(Pedido pedido);
    }
}
