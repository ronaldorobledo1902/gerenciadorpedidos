using GerenciadorPedidos.Api.ClientServices;
using GerenciadorPedidos.Api.ClientServices.Http;
using GerenciadorPedidos.Api.Service;
using GerenciadorPedidos.Api.Service.Interfaces;
using GerenciadorPedidos.Infra.Contexto;
using GerenciadorPedidos.Infra.Repositorio;
using GerenciadorPedidos.Infra.Repositorio.Interface;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorPedidos.Api.Configuracao
{
    public static class SqlConfigService
    {
        public static IServiceCollection AddSqlServerContext<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
        {
            return services.AddPooledDbContextFactory<SqlServerContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IItemRepositorio, ItemRepositorio>();
            services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
            services.AddScoped<IPedidoService, PedidoService>();

            return services;
        }
    }
}
