using System.Text.Json;

namespace GerenciadorPedidos.Api.Helpers
{
    public class JsonHelper
    {
        private readonly static JsonSerializerOptions options;

        static JsonHelper()
        {
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public static string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize<T>(value, options);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}
