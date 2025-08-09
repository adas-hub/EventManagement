using EventManagement.Application.DTOs.Events;
using EventManagement.Application.DTOs.Registrations;
using EventManagement.Application.DTOs.Users;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Enums;

namespace EventManagement.Application.Mappers;

public static class MappingConfig
{
    public static EventDto ToEventDto(this Event @event)
    {
        return new EventDto
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            StartDate = @event.StartDate,
            EndDate = @event.EndDate,
            Location = @event.Location,
            Status = @event.Status,
            MaxParticipants = @event.MaxParticipants,
            RegisteredParticipants = @event.Registrations.Count(r => r.Status == RegistrationStatus.Confirmed),
            OrganizerId = @event.OrganizerId,
            OrganizerUserName = @event.Organizer?.UserName ?? "Unknown Organizer",
            CreatedDate = @event.CreatedDate,
            UpdatedDate = @event.UpdatedDate
        };
    }

    public static UserDto ToUserDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate,
            Roles = user.Roles?.Select(r => r.Name).ToList() ?? new List<string>()
        };
    }
    public static RegistrationDto ToRegistrationDto(this Registration registration)
    {
        return new RegistrationDto
        {
            Id = registration.Id,
            EventId = registration.EventId,
            EventTitle = registration.Event.Title,
            EventDescription = registration.Event.Description,
            EventStartDate = registration.Event.StartDate,
            EventEndDate = registration.Event.EndDate,
            EventLocation = registration.Event.Location,
            EventStatus = registration.Event.Status,
            UserId = registration.UserId,
            UserName = registration.User.UserName,
            RegistrationDate = registration.CreatedDate,
            Status = registration.Status
        };
    }
}