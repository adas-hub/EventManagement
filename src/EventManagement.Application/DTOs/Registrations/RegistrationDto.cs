using EventManagement.Domain.Enums;

namespace EventManagement.Application.DTOs.Registrations;
public class RegistrationDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public string EventDescription { get; set; } = string.Empty;
    public DateTime EventStartDate { get; set; }
    public DateTime EventEndDate { get; set; }
    public string EventLocation { get; set; } = string.Empty;
    public EventStatus EventStatus { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
    public RegistrationStatus Status { get; set; }
}