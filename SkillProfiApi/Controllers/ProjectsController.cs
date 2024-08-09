using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillProfiApi.Models;

namespace SkillProfiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProjectsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var projects = await _context.Projects.ToListAsync();
            return Ok(projects);
        }

        // GET api/projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();
            return project;
        }

        // POST api/projects
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject([FromForm] Project projects)
        {
            if (projects.ImageFile != null)
            {
                string uniqueFileName = UploadedFile(projects.ImageFile);
                projects.ImageUrl = uniqueFileName;
            }

            var project = new Project
            {
                Title = projects.Title,
                Description = projects.Description,
                ImageUrl = projects.ImageUrl
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        // PUT api/projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, [FromForm] Project project)
        {
            if (id != project.Id) return BadRequest();

            var projectEntity = await _context.Projects.FindAsync(id);
            if (projectEntity == null) return NotFound();

            if (project.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(projectEntity.ImageUrl))
                {
                    string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", projectEntity.ImageUrl);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                string uniqueFileName = UploadedFile(project.ImageFile);
                projectEntity.ImageUrl = uniqueFileName;
            }

            projectEntity.Title = project.Title;
            projectEntity.Description = project.Description;

            _context.Entry(projectEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            if (!string.IsNullOrEmpty(project.ImageUrl))
            {
                string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", project.ImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return NoContent();
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
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
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
