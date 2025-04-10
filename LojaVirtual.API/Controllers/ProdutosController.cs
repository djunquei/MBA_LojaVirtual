﻿using LojaVirtual.Data;
using LojaVirtual.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/produtos")]
    public class ProdutosController : ControllerBase
    {
        private readonly LojaVirtualDbContext _context;

        public ProdutosController(LojaVirtualDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            if (_context.Produtos == null) return NotFound();

            return await _context.Produtos.ToListAsync();
        }

        [AllowAnonymous]
        [HttpGet("{idCategoria:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosPorCategoria(int idCategoria)
        {
            var produtos = await _context.Produtos.Where(x => x.CategoriaId == idCategoria).ToListAsync();

            if (_context.Produtos == null) return NotFound();

            return produtos;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post(Produto produto)
        {

            if (_context.Produtos == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Um ou mais erros de validação ocorreram!"
                });
            }
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Post), new { id = produto.Id }, produto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(int id, Produto produto)
        {
            if (id != produto.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Um ou mais erros de validação ocorreram!"
                });
            }

            _context.Produtos.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Produtos == null) return NotFound();

            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null) return NotFound();

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return (_context.Produtos?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}