namespace GerenciadorPedidos.Domain.Model
{
    public sealed class Material : Entity
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? Grupo { get; set; }
    }
}
