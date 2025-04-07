using System.Diagnostics;
using LojaVirtual.Data;
using LojaVirtual.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LojaVirtualDbContext _context;

        public HomeController(ILogger<HomeController> logger, LojaVirtualDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var lojaVirtualDbContext = _context.Produtos.Where(x => x.VendedorId == userId).Include(p => p.Categoria).Include(p => p.Vendedor);
            return View(await lojaVirtualDbContext.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
