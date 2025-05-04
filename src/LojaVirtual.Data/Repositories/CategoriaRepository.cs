using LojaVirtual.Data.Model;
using LojaVirtual.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly LojaVirtualDbContext _context;
        public CategoriaRepository(LojaVirtualDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> GetAll()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<Categoria> GetById(int id)
        {
            return await _context.Categorias.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddCategory(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategory(Categoria categoria)
        {
            _context.Categorias.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task DeleteCategory(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }

    }
}
