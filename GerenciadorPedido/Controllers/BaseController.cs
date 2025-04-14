using iHUB.Domain.Cqrs;
using iHUB.Domain.Cqrs.Handlers;
using iHUB.Domain.Dtos;
using iHUB.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Windows.Input;

namespace GerenciadorPedidos.Api.Controllers
{
    public class BaseController<TController> : ControllerBase where TController : class
    {
        private const string COMANDO_INVALIDO = "Comando inválido";
        private const string QUERY_INVALIDA = "Query inválida";

        private readonly ILogger<TController> _log;

        public BaseController(ILogger<TController> log)
        {
            _log = log;
        }

        protected async Task<ActionResult<TResult>> SendCommand<TComand, TResult>(ICommandHandler<TComand, TResult> handler, TComand command)
            where TComand : iHUB.Domain.Cqrs.ICommand, new()
            where TResult : ICommandResult, new()
        {
            ICommandResult res;
            int code;
            try
            {
                if (command == null)
                {
                    _log.LogError($"Command nulo|{COMANDO_INVALIDO}");
                    return BadRequest(
                        new
                        {
                            Error = new ErroCodigoDto
                            {
                                Codigo = "COMANDO_INVALIDO",
                                Mensagem = COMANDO_INVALIDO
                            }
                        });
                }

                if (string.IsNullOrWhiteSpace(command.Oid) || string.IsNullOrWhiteSpace(command.Id) || string.IsNullOrWhiteSpace(command.Sid))
                {
                    _log.LogError($"{command.GetType().FullName}|{COMANDO_INVALIDO}|E nescessarios passar os parametros Oid, Id e Sid");
                    return BadRequest(
                         new
                         {
                             Oid = command.Oid,
                             Id = command.Id,
                             Sid = command.Sid,
                             Error = new ErroCodigoDto
                             {
                                 Codigo = "COMANDO_INVALIDO",
                                 Mensagem = "É necessário passar os parametros Oid, Id e Sid"
                             }
                         });
                }

                if (handler == null)
                {
                    _log.LogError($"Handler nulo|Esta faltando alguma injeção de dependencia");
                    return BadRequest(
                        new
                        {
                            Oid = command.Oid,
                            Id = command.Id,
                            Sid = command.Sid,
                            Error = new ErroCodigoDto
                            {
                                Codigo = "HANDLER_INVALIDO",
                                Mensagem = "Handler inválido"
                            }
                        });
                }

                //loga o request
                _log.LogInformation($"{command.GetType().FullName}|{command.Id}|command: {JsonSerializer.Serialize(command)}");
                var result = await handler.Handle(command);
                if (result.Error != null)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (CodigoDescricaoException ex)
            {
                _log.LogError($"{command.GetType().FullName}|{command.Id}|error: {ex.Codigo}: {ex.Message}: {ex.Descricao}");
                code = StatusCodes.Status400BadRequest;
                res = new TResult
                {
                    Id = command.Id,
                    Oid = command.Oid,
                    Sid = command.Sid,
                    Error = new ErroCodigoDto
                    {
                        Codigo = ex.Codigo,
                        Mensagem = ex.Message
                    }
                };
            }
            catch (CodigoException ex)
            {
                _log.LogError($"{command.GetType().FullName}|{command.Id}|error: {ex.Codigo}: {ex.Message}");
                code = StatusCodes.Status400BadRequest;
                res = new TResult
                {
                    Id = command.Id,
                    Oid = command.Oid,
                    Sid = command.Sid,
                    Error = new ErroCodigoDto
                    {
                        Codigo = ex.Codigo,
                        Mensagem = ex.Message
                    }
                };
            }
            catch (ArgumentNullException ex)
            {
                _log.LogError($"{command.GetType().FullName}|{command.Id}|ArgumentNullException: {ex.Message}");
                code = StatusCodes.Status500InternalServerError;
                res = new TResult
                {
                    Id = command.Id,
                    Oid = command.Oid,
                    Sid = command.Sid,
                    Error = new ErroCodigoDto()
                    {
                        Codigo = "ArgumentNullException",
                        Mensagem = ex.Message
                    }
                };
            }
            catch (Exception ex)
            {
                _log.LogError($"{command.GetType().FullName}|{command.Id}|exception: {ex.Message}");
                code = StatusCodes.Status500InternalServerError;
                res = new TResult
                {
                    Id = command.Id,
                    Oid = command.Oid,
                    Sid = command.Sid,
                    Error = new ErroCodigoDto
                    {
                        Codigo = "Exception",
                        Mensagem = ex.Message
                    }
                };
            }

            return new ObjectResult(res)
            {
                StatusCode = code
            };
        }

        protected async Task<ActionResult<TResult>> SendQuery<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler, TQuery query)
          where TQuery : IQuery, new()
          where TResult : IQueryResult, new()
        {
            IQueryResult res;
            int code;
            try
            {
                if (query == null)
                {
                    _log.LogError($"Query nula|{QUERY_INVALIDA}");
                    return BadRequest(
                        new
                        {
                            Error = new ErroCodigoDto
                            {
                                Codigo = "QUERY_INVALIDA",
                                Mensagem = QUERY_INVALIDA
                            }
                        });
                }

                if (string.IsNullOrWhiteSpace(query.Oid) || string.IsNullOrWhiteSpace(query.Sid) || string.IsNullOrWhiteSpace(query.Id))
                {
                    _log.LogError($"{query?.GetType().FullName}|{QUERY_INVALIDA}|E nescessarios passar os parametros Oid, Id e Sid");
                    return BadRequest(
                         new
                         {
                             Oid = query.Oid,
                             Id = query.Id,
                             Sid = query.Sid,
                             Error = new ErroCodigoDto
                             {
                                 Codigo = "QUERY_INVALIDA",
                                 Mensagem = "É necessário passar os parametros Oid, Id e Sid"
                             }
                         });
                }

                if (handler == null)
                {
                    _log.LogError($"Handler nulo|Esta faltando alguma injeção de dependencia");
                    return BadRequest(
                        new
                        {
                            Oid = query.Oid,
                            Id = query.Id,
                            Sid = query.Sid,
                            Error = new ErroCodigoDto
                            {
                                Codigo = "HANDLER_INVALIDO",
                                Mensagem = "Handler inválido"
                            }
                        });
                }

                //loga o request
                _log.LogInformation($"{query.GetType().FullName}|{query.Sid}|query: {JsonSerializer.Serialize(query)}");

                var result = await handler.Handle(query);
                if (result.Error != null)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (CodigoDescricaoException ex)
            {
                _log.LogError($"{query.GetType().FullName}|Sid: {query.Sid}|error: {ex.Codigo}: {ex.Message}: {ex.Descricao}");
                code = StatusCodes.Status400BadRequest;
                res = new TResult
                {
                    Sid = query.Sid,
                    Oid = query.Oid,
                    Error = new ErroCodigoDto
                    {
                        Codigo = ex.Codigo,
                        Mensagem = ex.Message
                    }
                };
            }
            catch (CodigoException ex)
            {
                _log.LogError($"{query.GetType().FullName}|Sid: {query.Sid}|error: {ex.Codigo}: {ex.Message}");
                code = StatusCodes.Status400BadRequest;
                res = new TResult
                {
                    Sid = query.Sid,
                    Oid = query.Oid,
                    Error = new ErroCodigoDto
                    {
                        Codigo = ex.Codigo,
                        Mensagem = ex.Message
                    }
                };
            }
            catch (ArgumentNullException ex)
            {
                _log.LogError($"{query.GetType().FullName}|Sid: {query.Sid}|ArgumentNullException: {ex.Message}");
                code = StatusCodes.Status500InternalServerError;
                res = new TResult
                {
                    Sid = query.Sid,
                    Oid = query.Oid,
                    Error = new ErroCodigoDto()
                    {
                        Codigo = "ArgumentNullException",
                        Mensagem = ex.Message
                    }
                };
            }
            catch (Exception ex)
            {
                _log.LogError($"{query.GetType().FullName}|Sid: {query.Sid}|exception: {ex.Message}");
                code = StatusCodes.Status500InternalServerError;
                res = new TResult
                {
                    Sid = query.Sid,
                    Oid = query.Oid,
                    Error = new ErroCodigoDto()
                    {
                        Codigo = "Exception",
                        Mensagem = ex.Message
                    }
                };
            }

            return new ObjectResult(res)
            {
                StatusCode = code
            };
        }
    }
}
