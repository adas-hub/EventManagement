using EventManagement.Domain.Common;
using EventManagement.Domain.Common.Filters;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class EventRepository : Repository<Event>, IEventRepository
{
    public EventRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Event?> GetByIdWithOrganizerAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.Organizer)
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
    }
    public async Task<List<Event>> GetEventsByOrganizerIdAsync(Guid organizerId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.OrganizerId == organizerId)
            .Include(e => e.Organizer)
            .ToListAsync(cancellationToken);
    }

    public async Task<Event?> GetByIdWithRegistrationsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
    public async Task<PagedResult<Event>> GetEventsAsync(
      Paging paging,
      EventFilter filter,
      Sorting sorting,
      CancellationToken cancellationToken = default)
    {
        IQueryable<Event> query = _dbSet
            .Include(e => e.Organizer)
            .Include(e => e.Registrations);

        query = ApplyCommonEventFilters(query, filter);
        query = ApplyEventSorting(query, sorting);

        int totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((paging.PageNumber - 1) * paging.PageSize)
            .Take(paging.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Event>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = paging.PageNumber,
            PageSize = paging.PageSize
        };
    }

    private IQueryable<Event> ApplyCommonEventFilters(IQueryable<Event> query, EventFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string searchTermLower = filter.SearchTerm.ToLower();
            query = query.Where(e => e.Title.ToLower().Contains(searchTermLower) ||
                                     e.Description.ToLower().Contains(searchTermLower));
        }

        if (!string.IsNullOrWhiteSpace(filter.Location))
        {
            string locationLower = filter.Location.ToLower();
            query = query.Where(e => e.Location.ToLower().Contains(locationLower));
        }

        if (filter.StartDateFrom.HasValue)
        {
            query = query.Where(e => e.StartDate >= filter.StartDateFrom.Value);
        }
        if (filter.StartDateTo.HasValue)
        {
            query = query.Where(e => e.StartDate <= filter.StartDateTo.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(e => e.Status == filter.Status.Value);
        }

        if (filter.OrganizerId.HasValue)
        {
            query = query.Where(e => e.OrganizerId == filter.OrganizerId.Value);
        }

        return query;
    }

    private IQueryable<Event> ApplyEventSorting(IQueryable<Event> query, Sorting sorting)
    {
        switch (sorting.SortBy.ToLower())
        {
            case "startdate":
                query = sorting.SortOrder == "desc" ? query.OrderByDescending(e => e.StartDate) : query.OrderBy(e => e.StartDate);
                break;
            case "title":
                query = sorting.SortOrder == "desc" ? query.OrderByDescending(e => e.Title) : query.OrderBy(e => e.Title);
                break;
            case "location":
                query = sorting.SortOrder == "desc" ? query.OrderByDescending(e => e.Location) : query.OrderBy(e => e.Location);
                break;
            case "maxparticipants":
                query = sorting.SortOrder == "desc" ? query.OrderByDescending(e => e.MaxParticipants) : query.OrderBy(e => e.MaxParticipants);
                break;
            case "createddate":
            default:
                query = sorting.SortOrder == "desc" ? query.OrderByDescending(e => e.CreatedDate) : query.OrderBy(e => e.CreatedDate);
                break;
        }
        return query;
    }
}
