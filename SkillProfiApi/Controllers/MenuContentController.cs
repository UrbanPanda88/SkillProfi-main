using SkillProfiApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SkillProfiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuContentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MenuContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/menucontent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuTitles>> GetMenuTitle(int id)
        {
            var menuTitle = await _context.MenuTitles.FindAsync(id);
            if (menuTitle == null)
                return NotFound();

            return menuTitle;
        }

        // POST api/menucontent
        [HttpPost]
        public async Task<IActionResult> EditMenuTitle([FromBody] MenuTitles menuTitle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Update(menuTitle);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (!MenuTitleExists(menuTitle.Id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        private bool MenuTitleExists(int id)
        {
            return _context.MenuTitles.Any(e => e.Id == id);
        }
    }
}

