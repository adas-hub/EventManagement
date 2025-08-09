using EventManagement.Domain.Enums;

namespace EventManagement.Domain.Common.Filters;

public class EventFilter
{
    public string? SearchTerm { get; set; }
    public string? Location { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public EventStatus? Status { get; set; }
    public Guid? OrganizerId { get; set; }
}
