using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LojaVirtual.Data;
using LojaVirtual.Data.Model;
using Microsoft.AspNetCore.Authorization;

namespace LojaVirtual.UI.Controllers
{
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly LojaVirtualDbContext _context;

        public ProdutosController(LojaVirtualDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var lojaVirtualDbContext = _context.Produtos.Where(x => x.VendedorId == userId).Include(p => p.Categoria).Include(p => p.Vendedor);
            return View(await lojaVirtualDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descricao");
            ViewData["VendedorId"] = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao,Valor,Imagem,CategoriaId,VendedorId")] Produto produto)
        {
            ModelState.Remove("VendedorId");
            ModelState.Remove("Vendedor");
            ModelState.Remove("Categoria");
            if (ModelState.IsValid)
            {
                produto.VendedorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descricao", produto.CategoriaId);
            ViewData["VendedorId"] = new SelectList(_context.Vendedores, "Id", "Nome", produto.VendedorId);
            return View(produto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descricao", produto.CategoriaId);
            ViewData["VendedorId"] = new SelectList(_context.Vendedores, "Id", "Nome", produto.VendedorId);
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Valor,Imagem,CategoriaId,VendedorId")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descricao", produto.CategoriaId);
            ViewData["VendedorId"] = new SelectList(_context.Vendedores, "Id", "Nome", produto.VendedorId);
            return View(produto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
