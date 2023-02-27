using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DirectoryStructure.Controllers
{
    public class DirectoryController : Controller
    {
        private readonly DirectoryContext _context;

        public DirectoryController(DirectoryContext context)
            => _context = context;

        public IActionResult Import()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var directories = _context.Directories
                .Include(c => c.Children)
                .SingleOrDefault(c => c.Id == id);

            if (directories == null)
            {
                return NotFound();
            }

            return View(directories);
        }

        public IActionResult ImportFromOS(string path)
        {
            var root = new Models.Directory { Name = new DirectoryInfo(path).Name };
            AddSubdirectories(root, path);
            _context.Directories.Add(root);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        private void AddSubdirectories(Models.Directory parent, string path)
        {
            foreach (var subdirectoryPath in Directory.GetDirectories(path))
            {
                var subdirectory = new Models.Directory { Name = new DirectoryInfo(subdirectoryPath).Name, Parent = parent };
                AddSubdirectories(subdirectory, subdirectoryPath);
                if (parent.Children != null)
                {
                    parent.Children.Add(subdirectory);
                }
                else
                {
                    parent.Children = new List<Models.Directory>() { subdirectory };
                }
            }
        }
        
        public async Task<IActionResult> ImportDirectories(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<Models.Directory>().ToList();

                foreach (var record in records)
                {
                    var existingDirectory = await _context.Directories
                        .FirstOrDefaultAsync(d => d.Name == record.Name);

                    if (existingDirectory != null)
                    {
                        existingDirectory.ParentDirectoryId = record.ParentDirectoryId;
                    }
                    else
                    {
                        _context.Directories.Add(record);
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ExportDirectories()
        {
            var directories = await _context.Directories.ToListAsync();

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(directories);
                }

                return File(memoryStream.ToArray(), "text/csv", "directories.csv");
            }
        }
    }
}
