using EventManagement.Domain.Enums;

namespace EventManagement.Domain.Entities;

public class Registration : Entity
{    public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public Guid EventId { get; set; }
    public Event Event { get; set; } = default!;
    public DateTime? ApprovedDate { get; set; }
    public Guid? ApprovedByUserId { get; set; }
    public User? ApprovedByUser { get; set; }
    public DateTime? RejectedDate { get; set; }
    public Guid? RejectedByUserId { get; set; }
    public User? RejectedByUser { get; set; } 
    public string? RejectionReason { get; set; }
}