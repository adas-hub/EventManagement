using EventManagement.Application.DTOs.Events;

namespace EventManagement.Application.DTOs.Users;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
    public List<EventDto> OrganizedEvents { get; set; } = new List<EventDto>();
}
