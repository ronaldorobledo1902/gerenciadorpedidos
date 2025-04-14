using GerenciadorPedidos.Domain.Enum;
using GerenciadorPedidos.Domain.Model;
using GerenciadorPedidos.Infra.Contexto;
using GerenciadorPedidos.Infra.Repositorio.Interface;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorPedidos.Infra.Repositorio
{
    public class PedidoRepositorio :  IPedidoRepositorio
    {
        private readonly IDbContextFactory<SqlServerContext> _contextoFactory;

        public PedidoRepositorio(IDbContextFactory<SqlServerContext> contextoFactorycontexto)
        {
            _contextoFactory = contextoFactorycontexto;
        }


        public async Task<Pedido> Atualizar(Pedido Pedido)
        {
            using var _contexto = _contextoFactory.CreateDbContext();
            _contexto.Pedido.Update(Pedido);
            _contexto.SaveChanges();
            return Pedido;
        }


        public async Task<Pedido> Excluir(Pedido Pedido)
        {
            using var _contexto = _contextoFactory.CreateDbContext();
            _contexto.Pedido.Update(Pedido);
            await _contexto.SaveChangesAsync();
            return Pedido;
        }

        public async Task Inserir(Pedido Pedido)
        {
            using var _contexto = _contextoFactory.CreateDbContext();
            _contexto.Pedido.Add(Pedido);
            _contexto.SaveChanges();
        }

        public async Task<bool> AtualizarStatus(EnumStatusPedido enumStatusPedido)
        {
            using var _contexto = _contextoFactory.CreateDbContext();
            _contexto.Pedido
            .Where(p => p.Id == 1)
            .ExecuteUpdate(p => p.SetProperty(ped => ped.Status, enumStatusPedido));
            return true;
        }

        public async Task<Pedido> ObterPorId(int id)
        {
            using var _contexto = _contextoFactory.CreateDbContext();
            return await _contexto.Pedido.Include(i => i.Itens).Where(p => p.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<List<Pedido>> ObterPorStatus(EnumStatusPedido enumStatus)
        {
            using var _contexto = _contextoFactory.CreateDbContext();
            return await _contexto.Pedido.Include(i => i.Itens).Where(p => ((int) p.Status) ==  ((int)enumStatus)).AsNoTracking().ToListAsync();
        }

        public Task<List<Pedido>> ObterTodos()
        {
            using var _contexto = _contextoFactory.CreateDbContext();
            return _contexto.Pedido.Include(i => i.Itens).OrderBy(o => o.Id).AsNoTracking().ToListAsync();
        }



    }
}
