using EventManagement.Domain.Enums;

namespace EventManagement.Application.DTOs.Events;

public class EventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public EventStatus Status { get; set; }
    public int MaxParticipants { get; set; }
    public Guid OrganizerId { get; set; }
    public string OrganizerUserName { get; set; } = string.Empty;
    public int RegisteredParticipants { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}