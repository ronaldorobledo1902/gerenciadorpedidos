using GerenciadorPedidos.Api.Configuracao;
using GerenciadorPedidos.Api.Workers;
using GerenciadorPedidos.Infra.Contexto;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlServerContext<SqlServerContext>(configuration);
builder.Services.AddDomainServices();
builder.Services.ConfigureAutoMapper();
builder.Services.AddClientServices(configuration);
builder.Services.AddCqrsConfiguration();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

builder.Services.AddHostedService<PedidoCriadosProcessar>();

builder.Services.AddLoggingConfiguration(configuration);
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
