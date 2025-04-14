using GerenciadorPedidos.Domain.Enum;
using GerenciadorPedidos.Domain.Model;
using iHUB.Domain.Cqrs;
using iHUB.Domain.Dtos;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace GerenciadorPedidos.Domain.Commands
{
    public class PedidoInserirCommand : ICommand
    {
        
        public string Oid { get; set; }
        
        public string Id { get; set; }
        
        public string Sid { get; set; }
        public Pedido Pedido { get; set; }
    }


    public class PedidoInserirCommandResult : ICommandResult
    {
        public string Oid { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Sid
        /// </summary>
        public string Sid { get; set; }
        public EnumStatusPedido Status { get; set; }
        public ErroCodigoDto? Error { get; set; }

    }

}
