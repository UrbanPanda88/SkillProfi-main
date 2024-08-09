using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SkillProfi.Models;
using Microsoft.EntityFrameworkCore;
using SkillProfi;

public class MainMenuViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public MainMenuViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var menuTitles = await _context.MenuTitles.ToListAsync();

        var welcomeMessage = menuTitles
            .FirstOrDefault(mt => !string.IsNullOrWhiteSpace(mt.WelcomeMessage))
            ?.WelcomeMessage ?? "Добро пожаловать!";

        ViewData["WelcomeMessage"] = welcomeMessage;

        return View(menuTitles);
    }
}