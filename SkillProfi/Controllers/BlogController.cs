using SkillProfi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SkillProfi.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BlogController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.BlogPosts.ToListAsync();

            var projectPageTitle = await _context.MenuTitles
                                                 .Where(m => m.LinkController == "Blog" && m.LinkAction == "Index")
                                                 .Select(m => m.Name)
                                                 .FirstOrDefaultAsync();

            ViewData["MenuTitle"] = projectPageTitle ?? "Блог";

            return View(services);
        }

        public async Task<IActionResult> Post(int id)
        {
            var post = await _context.BlogPosts.FirstOrDefaultAsync(m => m.Id == id);
            if (post == null) return NotFound();
            return View(post);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,ImageFile")] BlogPost blogPost)
        {
            if (blogPost.ImageFile != null)
            {
                string uniqueFileName = UploadedFile(blogPost.ImageFile);
                blogPost.ImageUrl = uniqueFileName;
            }

            if (ModelState.IsValid)
            {
                blogPost.PublishDate = DateTime.Now;

                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(blogPost);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var project = await _context.BlogPosts.FindAsync(id);
            if (project == null) return NotFound();

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ImageUrl,Content,ImageFile")] BlogPost blogPost)
        {
            if (id != blogPost.Id) return NotFound();

            if (blogPost.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(blogPost.ImageUrl))
                {
                    string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", blogPost.ImageUrl);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                string uniqueFileName = UploadedFile(blogPost.ImageFile);
                blogPost.ImageUrl = uniqueFileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    blogPost.PublishDate = DateTime.UtcNow;
                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blogPost.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blogPost);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            return View(blogPost);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost != null)
            {
                if (!string.IsNullOrEmpty(blogPost.ImageUrl))
                {
                    string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", blogPost.ImageUrl);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.BlogPosts.Remove(blogPost);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }

        private string UploadedFile(IFormFile file)
        {
            string? uniqueFileName = null;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
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
}
