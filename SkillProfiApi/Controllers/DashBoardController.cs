using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillProfiApi.Models;

namespace SkillProfiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashBoardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DashBoard
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusInfo>>> GetRequests(DateTime? startDate, DateTime? endDate, string filter = "")
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

            var requestsByStatus = await requests.GroupBy(r => r.Status)
        .Select(g => new StatusInfo
        {
            Count = g.Count(),
            Requests = g.ToList(),
        })
        .ToListAsync();

            return requestsByStatus.Any() ? requestsByStatus : new List<StatusInfo>();
        }

        // GET: api/DashBoard/ChangeStatus/{id}
        [HttpGet("ChangeStatus/{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Request.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            return Ok();
        }

        // POST: api/DashBoard/ChangeStatus/{id}
        [HttpPost("ChangeStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] RequestStatus status)
        {
            var request = await _context.Request.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = status;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
