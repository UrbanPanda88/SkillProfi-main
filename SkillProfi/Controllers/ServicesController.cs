using SkillProfi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SkillProfi.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.Services.ToListAsync();

            var projectPageTitle = await _context.MenuTitles
                                                 .Where(m => m.LinkController == "Services" && m.LinkAction == "Index")
                                                 .Select(m => m.Name)
                                                 .FirstOrDefaultAsync();

            ViewData["MenuTitle"] = projectPageTitle ?? "Услуги";

            return View(services);
        }



        [Authorize]
        [HttpGet]
        public IActionResult AddService()
        {
            return View(new Service());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddService(Service newService)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(newService);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(newService);
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditService(int id)
        {
            var service = _context.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditService(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Update(service);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
