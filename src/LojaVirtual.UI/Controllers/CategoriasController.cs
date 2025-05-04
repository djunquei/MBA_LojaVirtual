using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LojaVirtual.Data.Model;
using Microsoft.AspNetCore.Authorization;
using LojaVirtual.Data.Repositories.Interfaces;

namespace LojaVirtual.Web.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IProdutoRepository _produtoRepository;

        public CategoriasController(ICategoriaRepository categoriaRepository, IProdutoRepository produtoRepository)
        {
            _categoriaRepository = categoriaRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _categoriaRepository.GetAll());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _categoriaRepository.GetById(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao")] Categoria categoria)
        {

            ModelState.Remove("Produtos");
            if (ModelState.IsValid)
            {
                await _categoriaRepository.AddCategory(categoria);
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _categoriaRepository.GetById(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Produtos");
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriaRepository.UpdateCategory(categoria);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CategoriaExists(categoria.Id))
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
            return View(categoria);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _categoriaRepository.GetById(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _categoriaRepository.GetById(id);
            if (categoria != null)
            {
                var produtos = await _produtoRepository.GetAllByCategoria(id);
                if (produtos != null && produtos.Any())
                {
                    ModelState.AddModelError("Id", "Erro ao tentar excluir esta categoria, existem produtos associados a ela.");
                    return View(categoria);
                }
                await _categoriaRepository.DeleteCategory(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoriaExists(int id)
        {
            var categoria = await _categoriaRepository.GetById(id);
            return categoria != null;
        }
    }
}
