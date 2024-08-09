using SkillProfi;
using SkillProfi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private HttpClient _httpClient;
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _httpClient = new HttpClient();
        _context = context;
    }

    public IActionResult Index()
    {
        var menuTitles = _context.MenuTitles.ToList();
        ViewData["MenuTitles"] = menuTitles;
        ViewData["ActionCallText"] = menuTitles.FirstOrDefault(mt => mt.ActionCallText != null)?.ActionCallText ?? string.Empty;
        ViewData["WelcomeMessage"] = menuTitles.FirstOrDefault(mt => mt.WelcomeMessage != null)?.WelcomeMessage ?? "Добро пожаловать!";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SubmitForm(string fullName, string email, string message)
    {
        var request = new Request
        {
            FullName = fullName,
            Email = email,
            Message = message,
            Status = RequestStatus.Received
        };

        request.RequestDate = DateTime.Now;

        _context.Request.Add(request);
        await _context.SaveChangesAsync();

        return RedirectToAction("Success");
    }


    [HttpGet]
    public IActionResult Success()
    {
        var pageContent = _context.MenuTitles.FirstOrDefault();
        return View();
    }

    [Authorize]
    [HttpGet]
    public IActionResult EditWelcomeMessage()
    {
        var welcomeMessage = _context.MenuTitles.FirstOrDefault(mt => mt.WelcomeMessage != null);
        if (welcomeMessage == null)
        {
            return NotFound();
        }
        return View(welcomeMessage);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> EditWelcomeMessage(MenuTitles menuTitle)
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

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        return View(menuTitle);
    }
}
