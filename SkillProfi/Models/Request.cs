using System;
using System.ComponentModel.DataAnnotations;

namespace SkillProfi.Models
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

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Message { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
    }
}