using System;

namespace SkillProfiWPF.Models
{
    public class RequestInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
