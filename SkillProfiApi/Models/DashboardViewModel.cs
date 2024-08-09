namespace SkillProfiApi.Models
{
    public class DashboardViewModel
    {
        public List<RequestInfo> Requests { get; set; }
    }

    public class StatusInfo
    {
        public int Count { get; set; }
        public List<Request>? Requests { get; set; }
    }
}
