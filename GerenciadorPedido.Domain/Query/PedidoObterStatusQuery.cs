using GerenciadorPedidos.Domain.Enum;
using GerenciadorPedidos.Domain.Model;
using iHUB.Domain.Cqrs;
using iHUB.Domain.Dtos;

namespace GerenciadorPedidos.Domain.Query
{
    public class PedidoObterStatusQuery : IQuery
    {
        public string Oid { get; set; }
        public string Id { get; set; }
        public string Sid { get; set; }
        public EnumStatusPedido status { get; set; }
    }

    public class PedidoObterStatusQueryResult : IQueryResult
    {
        public string Oid { get; set; }
        public string Id { get; set; }
        public string Sid { get; set; }
        public List<Pedido> Pedidos { get; set; }
        public ErroCodigoDto? Error { get; set; }
    }
}
