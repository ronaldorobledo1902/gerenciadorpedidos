using AutoMapper;
using GerenciadorPedidos.Domain.Commands;
using GerenciadorPedidos.Domain.Model;

namespace GerenciadorPedidos.Api.Configuracao
{
    public class MappingProfiler : Profile
    {
        public MappingProfiler() { 
        
            CreateMap<Pedido, PedidoInserirCommand>().ReverseMap();
        
        }
    }
}
