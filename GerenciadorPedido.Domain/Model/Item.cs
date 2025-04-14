using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorPedidos.Domain.Model
{
    public class Item : Entity
    {
        public int ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Valor { get; set; }

    }
}
