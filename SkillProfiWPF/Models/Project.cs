using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillProfiWPF.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
