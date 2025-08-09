using EventManagement.Domain.Common;
using EventManagement.Domain.Common.Filters;
using EventManagement.Domain.Entities;

namespace EventManagement.Domain.Repositories;

public interface IEventRepository : IRepository<Event>
{
    Task<Event?> GetByIdWithOrganizerAsync(Guid eventId, CancellationToken cancellationToken = default);
    Task<List<Event>> GetEventsByOrganizerIdAsync(Guid organizerId, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdWithRegistrationsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<Event>> GetEventsAsync(
    Paging paging,
    EventFilter filter,
    Sorting sorting,
    CancellationToken cancellationToken = default);

}
