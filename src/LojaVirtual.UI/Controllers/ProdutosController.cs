using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LojaVirtual.Data.Model;
using Microsoft.AspNetCore.Authorization;
using LojaVirtual.Web.Models;
using LojaVirtual.Data.Repositories.Interfaces;

namespace LojaVirtual.Web.Controllers
{
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public ProdutosController(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var lojaVirtualDbContext = await _produtoRepository.GetAllByVendedor(userId);
            return View(lojaVirtualDbContext);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _produtoRepository.GetById(id.Value);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        public async Task<IActionResult> Create()
        {
            var categorias = await _categoriaRepository.GetAll();
            ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Titulo");
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

                await _produtoRepository.AddProduct(produtoDb);

                return RedirectToAction(nameof(Index));
            }

            var categorias = await _categoriaRepository.GetAll();
            ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Titulo");
            return View(produto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _produtoRepository.GetById(id.Value);
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

            var categorias = await _categoriaRepository.GetAll();
            ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Titulo");

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

            var produtoDb = await _produtoRepository.GetById(id);

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

                    await _produtoRepository.UpdateProduct(produtoDb);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProdutoExists(id))
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

            var categorias = await _categoriaRepository.GetAll();
            ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Titulo", produto.CategoriaId);
            return View(produto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _produtoRepository.GetById(id.Value);

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
            await _produtoRepository.DeleteProduct(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProdutoExists(int id)
        {
            var produto = await _produtoRepository.GetById(id);
            return produto != null;
        }
    }
}
