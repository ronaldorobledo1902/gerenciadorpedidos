using GerenciadorPedidos.Domain.Model;
using GerenciadorPedidos.Infra.Contexto;
using GerenciadorPedidos.Infra.Repositorio.Interface;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorPedidos.Infra.Repositorio
{
    public class ItemRepositorio : IItemRepositorio
    {

        private readonly IDbContextFactory<SqlServerContext> _contextoFactory;
        public ItemRepositorio(IDbContextFactory<SqlServerContext> contextoFactorycontexto)
        {
            _contextoFactory = contextoFactorycontexto;
        }
    }
}
