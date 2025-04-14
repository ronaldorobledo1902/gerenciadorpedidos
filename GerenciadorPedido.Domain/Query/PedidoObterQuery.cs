using GerenciadorPedidos.Domain.Model;
using iHUB.Domain.Cqrs;
using iHUB.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GerenciadorPedidos.Domain.Query
{
    public class PedidoObterQuery : IQuery
    {
        
        public string Oid { get; set ; }
        
        public string Id { get ; set ; }
        
        public string Sid { get ; set ; }
        public int PedidoId { get; set; }
    }

    public class PedidoObterQueryResult : IQueryResult
    {
        [JsonIgnore]
        public string Oid { get; set; }
        [JsonIgnore]
        public string Id { get; set; }
        [JsonIgnore]
        public string Sid { get; set; }
        public Pedido Pedido { get; set; }
        public ErroCodigoDto? Error { get ; set ; }
    }

}
