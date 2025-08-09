using EventManagement.Domain.Common;
using EventManagement.Domain.Common.Filters;
using EventManagement.Domain.Entities;

namespace EventManagement.Domain.Repositories;

public interface IRegistrationRepository : IRepository<Registration>
{
    Task<Registration?> GetByEventAndUserAsync(Guid eventId, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> HasUserRegisteredForEventAsync(Guid eventId, Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<Registration>> GetUserRegistrationsAsync(
        Guid userId,
        Paging paging,
        RegistrationFilter filter,
        Sorting sorting,
        CancellationToken cancellationToken = default);
    Task<PagedResult<Registration>> GetEventRegistrationsAsync(
        Guid eventId,
        Paging paging,
        RegistrationFilter filter,
        Sorting sorting,
        CancellationToken cancellationToken = default);
}
