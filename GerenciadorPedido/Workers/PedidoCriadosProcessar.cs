
using GerenciadorPedidos.Api.ClientServices;
using GerenciadorPedidos.Api.Service.Interfaces;
using GerenciadorPedidos.Domain.Commands;
using GerenciadorPedidos.Domain.Enum;
using GerenciadorPedidos.Infra.Repositorio.Interface;

namespace GerenciadorPedidos.Api.Workers
{
    public class PedidoCriadosProcessar : BackgroundService
    {

        private readonly ILogger<PedidoCriadosProcessar> _logger;
        private IPedidoRepositorio _pedidoRepositorio;
        private readonly IServiceProvider _serviceProvider;
        private DateTime _nextRun = DateTime.UtcNow;
        private bool _estaProcessando = false;
        private IPedidoService _pedidoService;
        private ISistemaExternoHttp _sistemaExternoHttp;
        
        public PedidoCriadosProcessar(ILogger<PedidoCriadosProcessar> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            InternalAlterarProximaExecucao();

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _nextRun = DateTime.UtcNow.AddSeconds(20);
            _logger.LogInformation("Servico background registrado");
            _logger.LogInformation($"Proxima execucao: {_nextRun}");

            stoppingToken.Register(() =>
            {
                _logger.LogError("PEDIDO_CRIADO_PROCESSAR|Serviço cancelado por um evento externo.");
            });

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var delay = _nextRun - DateTime.UtcNow;
                    await Task.Delay(delay, stoppingToken);

                    try
                    {
                        await InternalExecutarTarefa(stoppingToken);

                    }
                    catch (Exception ex)
                    {
                        //tratamento porque não quero que o serviço pare.
                        _logger.LogError(ex, "PEDIDO_CRIADO_PROCESSAR|Erro crítico. Reiniciando serviço em 60 segundos...");
                    }
                    finally
                    {
                        InternalAlterarProximaExecucao();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("PEDIDO_CRIADO_PROCESSAR|Operação cancelada devido ao sinal de encerramento.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PEDIDO_CRIADO_PROCESSAR|Erro inesperado no serviço.");
            }
            finally
            {
                _logger.LogWarning("PEDIDO_CRIADO_PROCESSAR|Serviço foi finalizado.");
            }
        }


        private async Task InternalExecutarTarefa(CancellationToken stoppingToken)
        {
            if (_estaProcessando) return;

            try
            {
                _estaProcessando = true;

                using (var scope = _serviceProvider.CreateScope())
                {
                    _pedidoRepositorio = scope.ServiceProvider.GetRequiredService<IPedidoRepositorio>();
                    _pedidoService = scope.ServiceProvider.GetRequiredService<IPedidoService>();
                    //_sistemaExternoHttp = scope.ServiceProvider.GetRequiredService<ISistemaExternoHttp>();
                    await PedidosCriadosProcessar(stoppingToken);
                }

                _logger.LogInformation($"PEDIDO_CRIADO_PROCESSAR|[{DateTime.Now}] - Tarefa Finalizada.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PEDIDO_CRIADO_PROCESSAR| Erro no loop dos pedidos");
            }
            finally
            {
                _estaProcessando = false;
            }
            return;
        }


        private async Task PedidosCriadosProcessar(CancellationToken stoppingToken)
        {
            
            try
            {
                _logger.LogInformation($"PedidosCriadosProcessar|[{DateTime.Now}] - Executando a tarefa agendada.");
                
                var pedidos = await _pedidoRepositorio.ObterPorStatus(EnumStatusPedido.Criado);
                    
                await Parallel.ForEachAsync(pedidos, new ParallelOptions { MaxDegreeOfParallelism = 2, CancellationToken = stoppingToken }, async (pedido, token) =>
                {
                    try
                    {
                        pedido.Status = EnumStatusPedido.EmProcessamento;
                        await _pedidoRepositorio.AtualizarStatus(pedido.Status);

                        pedido = await _pedidoService.CalcularImposto(pedido);
                        pedido.Status = EnumStatusPedido.Processado;
                        await _pedidoRepositorio.Atualizar(pedido);

                        var pedidocommand = new PedidoInserirCommand()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Pedido = pedido,
                        };

                        await _sistemaExternoHttp.PedidoEnviar(pedidocommand);

                        _logger.LogInformation($"PEDIDO_CRIADOS_PROCESSAR|[{DateTime.Now}] - Pedido {pedido.Id} processado com sucesso .");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"PEDIDO_CRIADOS_PROCESSAR_ERROR|Inconsistência no processamento do pedido {pedido.Id} - Messagem: {ex.Message}");
                    }

                    _logger.LogInformation($"PEDIDO_CRIADOS_PROCESSAR|Pedido processao com sucesso : {pedido.Id}");
                });
            }
            catch (Exception ex)
            {

                _logger.LogError($"PEDIDO_CRIADOS_PROCESSAR_ERROR|Erro no processamento dos pedidos - messagem: {ex.Message}");
                throw;
            }
        }


        private async void OnProcessExit(object sender, EventArgs e)
        {
            _logger.LogError($"PEDIDO_CRIADOS_PROCESSAR_ERROR|Encerramento do processo detectado. Motivo: shutdown do aplicativo.");
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                _logger.LogError($"PEDIDO_CRIADOS_PROCESSAR_ERROR|Exceção crítica não tratada detectada.");
            }
            else
            {
                _logger.LogError($"PEDIDO_CRIADOS_PROCESSAR_ERROR|Exceção crítica não tratada detectada, mas sem detalhes disponíveis.");
            }
        }

        public override void Dispose()
        {
            _logger.LogInformation($"PEDIDO_CRIADOS_PROCESSAR_ERROR|Serviço está sendo descartado.");
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
            base.Dispose();
        }



        private void InternalAlterarProximaExecucao(DateTime? novaData = null)
        {
            novaData ??= DateTime.UtcNow.AddSeconds(20);
            _nextRun = novaData.Value;
        } 
    }
}
