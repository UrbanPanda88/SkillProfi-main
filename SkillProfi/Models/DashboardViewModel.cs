using SkillProfi.Models;

public class DashboardViewModel
{
    public Dictionary<RequestStatus, StatusInfo> RequestsByStatus { get; set; }
}

public class StatusInfo
{
    public int Count { get; set; }
    public List<Request> Requests { get; set; }
}
