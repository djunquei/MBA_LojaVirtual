using LojaVirtual.Data.Model;
using LojaVirtual.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly LojaVirtualDbContext _context;
        public ProdutoRepository(LojaVirtualDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> GetAll()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetAllByCategoria(int idCategoria)
        {
            return await _context.Produtos.Include(p => p.Categoria).Include(p => p.Vendedor).Where(x => x.CategoriaId == idCategoria).ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetAllByVendedor(string idVendedor)
        {
            return await _context.Produtos.Include(p => p.Categoria).Include(p => p.Vendedor).Where(x => x.VendedorId == idVendedor).ToListAsync();
        }

        public async Task<Produto> GetById(int id)
        {
            return await _context.Produtos.Include(p => p.Categoria).Include(p => p.Vendedor).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddProduct(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(Produto produto)
        {
            _context.Produtos.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task DeleteProduct(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }

    }
}
