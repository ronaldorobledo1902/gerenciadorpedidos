
using GerenciadorPedidos.Api.Handlers;
using GerenciadorPedidos.Domain.Commands;
using GerenciadorPedidos.Domain.Query;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace GerenciadorPedidos.Api.Controllers
{
    [ApiController]
    [Route("v1/pedido")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class PedidoController : BaseController<PedidoController>
    {
        public PedidoController(ILogger<PedidoController> log) : base(log)
        {
        }


        [HttpPost("inserir")]
        [ProducesResponseType(typeof(PedidoInserirCommandResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PedidoInserirCommandResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PedidoInserirCommandResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PedidoInserirCommandResult), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PedidoInserirCommandResult>> PedidoInserir([FromServices] PedidoInserirHandler handler,
          [FromBody] PedidoInserirCommand command)
        {
            command.Oid = Guid.NewGuid().ToString();
            command.Sid = Guid.NewGuid().ToString();
            command.Id = Guid.NewGuid().ToString();
            return await SendCommand(handler, command);
        }
            


        [HttpGet("obter")]
        [ProducesResponseType(typeof(PedidoObterQueryResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PedidoObterQueryResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PedidoObterQueryResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PedidoObterQueryResult), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PedidoObterQueryResult>> PedidoObter([FromServices] PedidoObterQueryHandler handler,
          [FromQuery] PedidoObterQuery command)
        {
            command.Oid = Guid.NewGuid().ToString();
            command.Sid = Guid.NewGuid().ToString();
            command.Id = Guid.NewGuid().ToString();
            return await SendQuery(handler, command);
        }
        


        [HttpGet("obter/status")]
        [ProducesResponseType(typeof(PedidoObterStatusQueryResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PedidoObterStatusQueryResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PedidoObterStatusQueryResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PedidoObterStatusQueryResult), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PedidoObterStatusQueryResult>> PedidoObterStatus([FromServices] PedidoObterStatusQueryHandler handler,
          [FromQuery] PedidoObterStatusQuery command)
        {
            command.Oid = Guid.NewGuid().ToString();
            command.Sid = Guid.NewGuid().ToString();
            command.Id = Guid.NewGuid().ToString();
            return await SendQuery(handler, command);
        }
            


        [HttpPost("simuladorurlexterna")]
        [ProducesResponseType(typeof(PedidoInserirCommandResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PedidoInserirCommandResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PedidoInserirCommandResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PedidoInserirCommandResult), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PedidoInserirCommandResult>> SimuladorUrlExterna([FromBody] PedidoInserirCommand command)
        {
            return new PedidoInserirCommandResult();                    
        } 

    }
}
