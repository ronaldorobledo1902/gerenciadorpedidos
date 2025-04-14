using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorPedidos.Domain.Enum
{
    public enum EnumStatusPedido : int
    {
        EmProcessamento = 1,
        Cancelado = 2,
        Recusado = 3,
        Processado = 4,
        Criado = 5
    }
}
