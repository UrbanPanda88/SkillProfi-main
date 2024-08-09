using System;
using System.ComponentModel.DataAnnotations;

namespace SkillProfiApi.Models
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

        public string ?FullName { get; set; }

        [EmailAddress]
        public string ?Email { get; set; }

        public string ?Message { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
    }
}