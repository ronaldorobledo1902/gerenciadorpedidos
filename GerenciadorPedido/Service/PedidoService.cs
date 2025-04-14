using GerenciadorPedidos.Api.Service.Interfaces;
using GerenciadorPedidos.Domain.Model;

namespace GerenciadorPedidos.Api.Service
{
    public class PedidoService : IPedidoService
    {
        private string? TipoImposto { get; set; }

        public PedidoService(IConfiguration configuration)
        {
            TipoImposto = configuration["NovoImposto"];
        }

        public async Task<Pedido> CalcularImposto(Pedido pedido)
        {
            pedido.ValorTotal = pedido.Itens.Sum(p => p.Valor * p.Quantidade);
            if (TipoImposto.Contains("ImpostoAtual"))
                pedido.Imposto = pedido.ValorTotal * 0.3m;
            else
                pedido.Imposto = pedido.ValorTotal * 0.2m;


            return pedido;
        }
    }
}
