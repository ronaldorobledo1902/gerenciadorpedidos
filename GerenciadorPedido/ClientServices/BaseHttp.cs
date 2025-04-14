using iHUB.Domain.Cqrs;
using iHUB.Domain.Exceptions;
using System.Net.Mime;
using System.Net;
using System.Text;
using GerenciadorPedidos.Api.Helpers;
using Newtonsoft.Json;

namespace GerenciadorPedidos.Api.ClientServices
{
    public class BaseHttp<T>
    {
        private readonly HttpClient _client;
        private readonly ILogger<T> _log;

        public BaseHttp(HttpClient client, ILogger<T> log)
        {
            _client = client;
            _log = log;
        }

        /// <summary>
        /// Faz o POST para o Command ou Query em outro serviço do iHUB.
        /// </summary>
        /// <params>
        /// <param name="code">é o código de Erro base quando existir</param>
        /// <param name="url">request endpoint</param>
        /// <param name="request">command ou query da request</param>
        /// </params>
        public async Task<TResult> PostAsync<TRequest, TResult>(string code, string url, TRequest request)
            where TRequest : IBase
            where TResult : IBaseResult
        {
            if (code == null)
                throw new ArgumentException("erroCode");
            if (url == null)
                throw new ArgumentException("url");
            if (request == null)
                throw new ArgumentException("request");
            string? json = null;
            try
            {
                var jsonBody = JsonHelper.Serialize<TRequest>(request);
                var content = new StringContent(jsonBody, Encoding.UTF8, MediaTypeNames.Application.Json);
                var response = await _client.PostAsync(url, content);
                json = await response.Content.ReadAsStringAsync();
                // se json for null ou não for JSON (nao começa tem {}) então devemos logar o response
                if (string.IsNullOrEmpty(json) || !json.StartsWith("{"))
                    throw new CodigoDescricaoException(code + "_SERVICE",
                        $"Response retornando não é um JSON: request {_client.BaseAddress}{url}: {json}",
                        "Ocorreu um erro de serviço na requisição; invalid response");
                //var result = JsonHelper.Deserialize<TResult>(json);
                var result = JsonConvert.DeserializeObject<TResult>(json);
                if (result == null)
                    throw new CodigoDescricaoException(code, "response content is null", "falhou na resposta do servico");
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (result.Error != null)
                        throw new CodigoException(result.Error.Codigo!, result.Error.Mensagem!);
                    else
                        throw new CodigoException(code + "_RESPONSE", json);
                }
                return result;
            }
            catch (JsonException ex)
            {
                throw new CodigoDescricaoException(code + "_JSON",
                    $"Serviço falhou ao gerar código de result: {_client.BaseAddress}{url}; json retornado {json}; {ex.Message}",
                    "Ocorreu um erro de serviço na requisição");
            }
            catch (HttpRequestException ex)
            {
                throw new CodigoDescricaoException(code + "_NETWORK",
                    $"Falhou ao contactar servico: {_client.BaseAddress} -> {ex.Message}",
                    "Ocorreu um erro de serviço na requisição");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
