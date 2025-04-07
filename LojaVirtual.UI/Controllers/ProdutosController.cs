using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LojaVirtual.Data;
using LojaVirtual.Data.Model;
using Microsoft.AspNetCore.Authorization;
using LojaVirtual.UI.Models;

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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Titulo");
            ViewData["VendedorId"] = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao,Valor,Estoque,Imagem,CategoriaId")] ProdutoViewModel produto)
        {
            Produto produtoDb = new Produto();
            if (ModelState.IsValid)
            {
                if (produto.Imagem != null && produto.Imagem.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + produto.Imagem.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await produto.Imagem.CopyToAsync(stream);
                    }

                    produtoDb.Imagem = "/images/" + uniqueFileName;
                }

                produtoDb.VendedorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                produtoDb.Titulo = produto.Titulo;
                produtoDb.Descricao = produto.Descricao;
                produtoDb.Valor = produto.Valor;
                produtoDb.Estoque = produto.Estoque;
                produtoDb.CategoriaId = produto.CategoriaId;

                _context.Add(produtoDb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Titulo");
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

            ProdutoViewModel produtoViewModel = new ProdutoViewModel();
            produtoViewModel.Id = produto.Id;
            produtoViewModel.Titulo = produto.Titulo;
            produtoViewModel.Descricao = produto.Descricao;
            produtoViewModel.Valor = produto.Valor;
            produtoViewModel.Estoque = produto.Estoque;
            produtoViewModel.CategoriaId = produto.CategoriaId;

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Titulo");
            
            return View(produtoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Valor,Estoque,Imagem,CategoriaId")] ProdutoViewModel produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            var produtoDb = _context.Produtos.Find(id);

            if (produtoDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (produto.Imagem != null && produto.Imagem.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + produto.Imagem.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await produto.Imagem.CopyToAsync(stream);
                        }

                        produtoDb.Imagem = "/images/" + uniqueFileName;
                    }

                    produtoDb.Titulo = produto.Titulo;
                    produtoDb.Descricao = produto.Descricao;
                    produtoDb.Valor = produto.Valor;
                    produtoDb.Estoque = produto.Estoque;
                    produtoDb.CategoriaId = produto.CategoriaId;

                    _context.Update(produtoDb);
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Titulo", produto.CategoriaId);
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
