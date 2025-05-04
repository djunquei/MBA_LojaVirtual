using LojaVirtual.Data.Model;
using LojaVirtual.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IProdutoRepository _produtoRepository;

        public CategoriasController(ICategoriaRepository categoriaRepository, IProdutoRepository produtoRepository)
        {
            _categoriaRepository = categoriaRepository;
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {
            var categorias = await _categoriaRepository.GetAll();
            if (categorias == null) return NotFound();

            return categorias.ToList();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post(Categoria categoria)
        {
            var categorias = await _categoriaRepository.GetAll();
            if (categorias == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Um ou mais erros de validação ocorreram!"
                });
            }
            await _categoriaRepository.AddCategory(categoria);

            return CreatedAtAction(nameof(Post), new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(int id, Categoria categoria)
        {
            if (id != categoria.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Um ou mais erros de validação ocorreram!"
                });
            }

            try
            {
                await _categoriaRepository.UpdateCategory(categoria);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await CategoriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _categoriaRepository.GetById(id);
            if (categoria != null)
            {
                var produtos = await _produtoRepository.GetAllByCategoria(id);
                if (produtos != null && produtos.Any())
                {
                    return ValidationProblem(new ValidationProblemDetails(ModelState)
                    {
                        Title = "Erro ao tentar excluir esta categoria, existem produtos associados a ela."
                    });
                }
                await _categoriaRepository.DeleteCategory(id);
            }

            return NoContent();
        }

        private async Task<bool> CategoriaExists(int id)
        {
            var categoria = await _categoriaRepository.GetById(id);
            return categoria != null;
        }

    }
}