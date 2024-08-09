using SkillProfi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SkillProfi.Controllers
{
    [Authorize]
    public class MenuContentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> EditMenuTitle(int id)
        {
            var menuTitle = await _context.MenuTitles.FindAsync(id);
            return View(menuTitle);
        }

        [HttpPost]
        public async Task<IActionResult> EditMenuTitle(MenuTitles menuTitle)
        {
            if (ModelState.IsValid)
            {
                _context.Update(menuTitle);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(menuTitle);
        }
    }
}
