using GerenciadorPedidos.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorPedidos.Infra.Contexto
{
    public class SqlServerContext : DbContext
    {
        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        {
            
            this.Database.EnsureCreated();
        }

        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Item> Item { get; set; }
    }
}
