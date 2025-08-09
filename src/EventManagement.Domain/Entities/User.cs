namespace EventManagement.Domain.Entities;

public class User : Entity
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
