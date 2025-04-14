using GerenciadorPedidos.Domain.Enum;

namespace GerenciadorPedidos.Domain.Model
{
    public class Pedido : Entity
    {
        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal Imposto { get; set; }
        public List<Item> Itens { get; set; }
        public EnumStatusPedido Status { get; set; }

    }
}
