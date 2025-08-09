using EventManagement.Domain.Enums;

namespace EventManagement.Domain.Common.Filters;

public class RegistrationFilter
{
    public string? SearchTerm { get; set; }
    public RegistrationStatus? Status { get; set; }
    public EventStatus? EventStatus { get; set; }
}
