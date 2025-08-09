using EventManagement.Domain.Common;
using EventManagement.Domain.Common.Filters;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
{
    public RegistrationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Registration?> GetByEventAndUserAsync(Guid eventId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.Event)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId, cancellationToken);
    }

    public async Task<bool> HasUserRegisteredForEventAsync(Guid eventId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(r => r.EventId == eventId && r.UserId == userId, cancellationToken);
    }
    public async Task<PagedResult<Registration>> GetEventRegistrationsAsync(
           Guid eventId,
           Paging paging,
           RegistrationFilter filter,
           Sorting sorting,
           CancellationToken cancellationToken = default)
    {
        IQueryable<Registration> query = _dbSet
            .Where(r => r.EventId == eventId)
            .Include(r => r.Event)
            .Include(r => r.User);

        query = ApplyRegistrationFilters(query, filter);
        query = ApplyRegistrationSorting(query, sorting);

        int totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((paging.PageNumber - 1) * paging.PageSize)
            .Take(paging.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Registration>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = paging.PageNumber,
            PageSize = paging.PageSize
        };
    }
    public async Task<PagedResult<Registration>> GetUserRegistrationsAsync(
        Guid userId,
        Paging paging,
        RegistrationFilter filter,
        Sorting sorting,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Registration> query = _dbSet
            .Where(r => r.UserId == userId)
            .Include(r => r.Event)
            .Include(r => r.User);

        query = ApplyRegistrationFilters(query, filter);
        query = ApplyRegistrationSorting(query, sorting);

        int totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((paging.PageNumber - 1) * paging.PageSize)
            .Take(paging.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Registration>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = paging.PageNumber,
            PageSize = paging.PageSize
        };
    }

    private IQueryable<Registration> ApplyRegistrationFilters(IQueryable<Registration> query, RegistrationFilter filter)
    {
        if (filter == null) return query;

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string searchTermLower = filter.SearchTerm.ToLower();
            query = query.Where(r => r.Event.Title.ToLower().Contains(searchTermLower) ||
                                     r.Event.Description.ToLower().Contains(searchTermLower));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(r => r.Status == filter.Status.Value);
        }

        if (filter.EventStatus.HasValue)
        {
            query = query.Where(r => r.Event.Status == filter.EventStatus.Value);
        }

        return query;
    }
    private IQueryable<Registration> ApplyRegistrationSorting(IQueryable<Registration> query, Sorting sorting)
    {
        bool isDescending = string.Equals(sorting.SortOrder, "desc", StringComparison.OrdinalIgnoreCase);

        switch (sorting.SortBy.ToLowerInvariant())
        {
            case "registrationdate":
                query = isDescending ? query.OrderByDescending(r => r.CreatedDate) : query.OrderBy(r => r.CreatedDate);
                break;
            case "status":
                query = isDescending ? query.OrderByDescending(r => r.Status) : query.OrderBy(r => r.Status);
                break;
            case "eventtitle":
                query = isDescending ? query.OrderByDescending(r => r.Event.Title) : query.OrderBy(r => r.Event.Title);
                break;
            case "eventstartdate":
                query = isDescending ? query.OrderByDescending(r => r.Event.StartDate) : query.OrderBy(r => r.Event.StartDate);
                break;
            case "eventenddate":
                query = isDescending ? query.OrderByDescending(r => r.Event.EndDate) : query.OrderBy(r => r.Event.EndDate);
                break;
            case "eventstatus":
                query = isDescending ? query.OrderByDescending(r => r.Event.Status) : query.OrderBy(r => r.Event.Status);
                break;
            case "eventlocation":
                query = isDescending ? query.OrderByDescending(r => r.Event.Location) : query.OrderBy(r => r.Event.Location);
                break;
            case "createddate":
                query = isDescending ? query.OrderByDescending(r => r.CreatedDate) : query.OrderBy(r => r.CreatedDate);
                break;
            default:
                query = isDescending ? query.OrderByDescending(r => r.CreatedDate) : query.OrderBy(r => r.CreatedDate);
                break;
        }
        return query;
    }
}