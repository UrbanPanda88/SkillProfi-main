using System.ComponentModel.DataAnnotations;

namespace SkillProfiWPF.Models
{
    public enum RequestStatus
    {
        Received,
        InProgress,
        Completed,
        Rejected,
        Canceled
    }

    public class Request
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
        public RequestStatus Status { get; set; }
        public System.DateTime RequestDate { get; set; }
    }
}