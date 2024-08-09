using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillProfi.Models;
using SkillProfi;

public class ProjectsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProjectsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
    }

     public async Task<IActionResult> Index()
     {
            var services = await _context.Projects.ToListAsync();

            var projectPageTitle = await _context.MenuTitles
                                                 .Where(m => m.LinkController == "Projects" && m.LinkAction == "Index")
                                                 .Select(m => m.Name)
                                                 .FirstOrDefaultAsync();

            ViewData["MenuTitle"] = projectPageTitle ?? "Проекты";

            return View(services);
     }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        if (project == null) return NotFound();

        return View(project);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description,ImageFile")] Project project)
    {
        if (project.ImageFile != null)
        {
            string uniqueFileName = UploadedFile(project.ImageFile);
            project.ImageUrl = uniqueFileName;
        }

        if (ModelState.IsValid)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(project);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return NotFound();

        return View(project);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ImageUrl,ImageFile")] Project project)
    {
        if (id != project.Id) return NotFound();

        if (project.ImageFile != null)
        {
            if (!string.IsNullOrEmpty(project.ImageUrl))
            {
                string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", project.ImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            string uniqueFileName = UploadedFile(project.ImageFile);
            project.ImageUrl = uniqueFileName;
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(project);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (!string.IsNullOrEmpty(project.ImageUrl))
        {
            string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", project.ImageUrl);
            System.IO.File.Delete(filePath);
        }
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.Id == id);
    }

    private string UploadedFile(IFormFile file)
    {
        string? uniqueFileName = null;

        if (file != null)
        {
            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }
        return uniqueFileName;
    }
}
