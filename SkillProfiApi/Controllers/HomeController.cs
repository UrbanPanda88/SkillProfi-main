using SkillProfiApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace SkillProfiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("MenuTitles")]
        public IActionResult GetMenuTitles()
        {
            var menuTitles = _context.MenuTitles.ToList();
            return Ok(menuTitles);
        }

        [HttpPost]
        [Route("SubmitForm")]
        public async Task<IActionResult> SubmitForm([FromBody] Request request)
        {
            if (ModelState.IsValid)
            {
                request.RequestDate = DateTime.Now;
                _context.Request.Add(request);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost]
        [Route("EditWelcomeMessage")]
        public async Task<IActionResult> EditWelcomeMessage([FromBody] MenuTitles menuTitle)
        {
            if (ModelState.IsValid)
            {
                var existingMenuTitle = await _context.MenuTitles
                    .FirstOrDefaultAsync(mt => mt.Id == menuTitle.Id);
                if (existingMenuTitle != null)
                {
                    existingMenuTitle.WelcomeMessage = menuTitle.WelcomeMessage;
                    existingMenuTitle.ActionCallText = menuTitle.ActionCallText;
                    _context.MenuTitles.Update(existingMenuTitle);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest(ModelState);
        }
    }
}
