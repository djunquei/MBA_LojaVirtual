using LojaVirtual.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Data
{
    public class LojaVirtualDbContext : DbContext
    {
        public LojaVirtualDbContext(DbContextOptions<LojaVirtualDbContext> options) : base(options)
        {
        }

        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }

    }
}
