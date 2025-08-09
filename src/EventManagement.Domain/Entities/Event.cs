using EventManagement.Domain.Enums;

namespace EventManagement.Domain.Entities;

public class Event : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public EventStatus Status { get; set; } = EventStatus.Draft;
    public int MaxParticipants { get; set; }
    public int RegisteredParticipantsCount { get; set; } = 0;
    public Guid OrganizerId { get; set; }
    public User Organizer { get; set; } = default!;
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
