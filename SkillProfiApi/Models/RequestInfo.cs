namespace SkillProfiApi.Models
{
    public class RequestInfo
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
