using AutoMapper;
using GerenciadorPedidos.Api.ClientServices;
using GerenciadorPedidos.Api.ClientServices.Http;
using iHUB.Domain.Cqrs.Handlers;
using Serilog;
using System.Reflection;

namespace GerenciadorPedidos.Api.Configuracao
{
    public static class ConfigurationService
    {

        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiler());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }

        public static IServiceCollection AddClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (string.IsNullOrEmpty(configuration.GetValue<string>("SistemaExternoBase").ToString()))
                throw new ArgumentNullException("SistemaExternoBase");
            
            services.AddHttpClient<ISistemaExternoHttp, SistemaExternoHttp>(c => {
                c.BaseAddress = new Uri(configuration.GetValue<string>("SistemaExternoBase").ToString());
            });
            
            return services;
        }

        public static IServiceCollection AddCqrsConfiguration(this IServiceCollection services)
        {
            var handlerTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => !type.IsAbstract && (type.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) || x.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))));

            foreach (var handlerType in handlerTypes)
                services.AddScoped(handlerType);

            return services;
        }


        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration) // lê do appsettings
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        //.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                        .CreateLogger();

            return services;
        }
    }
}
