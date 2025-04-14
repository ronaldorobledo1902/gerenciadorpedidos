using GerenciadorPedidos.Domain.Enum;
using GerenciadorPedidos.Domain.Model;

namespace GerenciadorPedidos.Infra.Repositorio.Interface
{
    public interface IPedidoRepositorio
    {
        Task<Pedido> Excluir(Pedido Pedido);
        Task Inserir(Pedido Pedido);
        Task<Pedido> ObterPorId(int id);
        Task<List<Pedido>> ObterTodos();
        Task<List<Pedido>> ObterPorStatus(EnumStatusPedido enumStatus);
        Task<bool> AtualizarStatus(EnumStatusPedido enumStatusPedido);
        Task<Pedido> Atualizar(Pedido Pedido);
    }
}
