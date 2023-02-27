using DirectoryStructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DirectoryStructure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DirectoryContext _context;

        public HomeController(ILogger<HomeController> logger,
            DirectoryContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var directories = _context.Directories
                .Where(c => c.ParentDirectoryId == null)
                .Include(c => c.Children)
                .ToList();

            return View(directories);
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