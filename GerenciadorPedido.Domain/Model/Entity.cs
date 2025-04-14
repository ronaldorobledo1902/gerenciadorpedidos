using System.Text.Json.Serialization;

namespace GerenciadorPedidos.Domain.Model
{
    public class Entity : IEntity
    {
        [JsonIgnore]
        public int Id { get ; set ; }
    }
}
