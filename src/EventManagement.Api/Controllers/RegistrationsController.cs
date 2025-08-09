using EventManagement.Application.Features.Registrations.ApproveRegistration;
using EventManagement.Application.Features.Registrations.GetEventParticipants;
using EventManagement.Application.Features.Registrations.GetMyRegistrations;
using EventManagement.Application.Features.Registrations.RegisterForEvent;
using EventManagement.Application.Features.Registrations.RejectRegistration;
using EventManagement.Application.Features.Registrations.UnregisterFromEvent;
using EventManagement.Domain.Common;
using EventManagement.Domain.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Api.Controllers;

public class RegistrationsController : BaseController
{
    [HttpPost("{eventId:guid}/register")]
    [Authorize(Roles = "User, Attendee")] 
    public async Task<IActionResult> RegisterForEvent(Guid eventId)
    {
        var command = new RegisterForEventCommand(eventId, UserId!.Value);
        var result = await Sender.Send(command);

        return HandleResult(result); 
    }
    [HttpDelete("{eventId:guid}/unregister")]
    [Authorize(Roles = "User, Attendee")]
    public async Task<IActionResult> UnregisterFromEvent(Guid eventId)
    {
        var command = new UnregisterFromEventCommand(eventId, UserId!.Value);
        var result = await Sender.Send(command);

        return HandleResult(result);
    }
    [HttpGet("my-registrations")]
    [Authorize(Roles = "User, Attendee")]
    public async Task<IActionResult> GetMyRegistrations(
        [FromQuery] Paging paging,
        [FromQuery] RegistrationFilter filter,
        [FromQuery] Sorting sorting,
        CancellationToken cancellationToken)
    {
        var query = new GetMyRegistrationsQuery(UserId!.Value, paging, filter, sorting);
        var result = await Sender.Send(query, cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("{eventId:guid}/participants")]
    [Authorize(Roles = "Organizer, Admin")]
    public async Task<IActionResult> GetEventParticipants(
        Guid eventId,
        [FromQuery] Paging paging,
        [FromQuery] RegistrationFilter filter,
        [FromQuery] Sorting sorting,
        CancellationToken cancellationToken)
    {
        var query = new GetEventParticipantsQuery(eventId, paging, filter, sorting);
        var result = await Sender.Send(query, cancellationToken);

        return HandleResult(result);
    }
    [HttpPut("{registrationId:guid}/approve")]
    [Authorize(Roles = "Organizer, Admin")]
    public async Task<IActionResult> ApproveRegistration(Guid registrationId)
    {
        var command = new ApproveRegistrationCommand(registrationId, UserId!.Value);
        var result = await Sender.Send(command);

        return HandleResult(result);
    }

    [HttpPut("{registrationId:guid}/reject")] 
    [Authorize(Roles = "Organizer, Admin")]
    public async Task<IActionResult> RejectRegistration(Guid registrationId, [FromBody] RejectRegistrationCommand command)
    {
        if (registrationId != command.RegistrationId)
        {
            return HandleResult(Error.Failure("Validation.RegistrationIdMismatch", "Registration ID in route must match ID in the request body."));
        }

        var commandWithRejector = command with { RejectingUserId = UserId!.Value };
        var result = await Sender.Send(commandWithRejector);

        return HandleResult(result);
    }
}
