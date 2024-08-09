using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillProfi.Models;
using System.Linq;
using SkillProfi;

public class DashBoardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashBoardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string filter = "")
    {
        var requests = _context.Request.AsQueryable();

        DateTime today = DateTime.Today;
        switch (filter.ToLower())
        {
            case "today":
                startDate = today;
                endDate = today;
                break;
            case "yesterday":
                startDate = today.AddDays(-1);
                endDate = today.AddDays(-1);
                break;
            case "week":
                startDate = today.AddDays(-((int)today.DayOfWeek + 6) % 7);
                endDate = today;
                break;
            case "month":
                startDate = new DateTime(today.Year, today.Month, 1);
                endDate = startDate.Value.AddMonths(1).AddDays(-1);
                break;
        }

        if (startDate.HasValue && endDate.HasValue)
        {
            requests = requests.Where(r => r.RequestDate.Date >= startDate.Value && r.RequestDate.Date <= endDate.Value);
        }

        var viewModel = new DashboardViewModel
        {
            RequestsByStatus = await requests.GroupBy(r => r.Status)
                                             .ToDictionaryAsync(g => g.Key, g => new StatusInfo { Count = g.Count(), Requests = g.ToList() }),
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        var request = await _context.Request.FindAsync(id);
        if (request == null)
        {
            return NotFound();
        }

        return View(request);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeStatus(int id, RequestStatus status)
    {
        var request = await _context.Request.FindAsync(id);
        if (request != null)
        {
            request.Status = status;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
