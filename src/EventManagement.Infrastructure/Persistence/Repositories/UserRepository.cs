using EventManagement.Domain.Common;
using EventManagement.Domain.Common.Filters;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByIdWithRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
                     .Include(u => u.Roles)
                     .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByIdWithRolesAndEventsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
                     .Include(u => u.Roles)
                     .Include(u => u.OrganizedEvents)
                     .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }
    public async Task<PagedResult<User>> GetUsersAsync(
        Paging paging,
        UserFilter filter,
        Sorting sorting,
        CancellationToken cancellationToken = default)
    {
        IQueryable<User> query = _dbSet.Include(u => u.Roles);

        query = ApplyUserFilters(query, filter);

        query = ApplyUserSorting(query, sorting);

        int totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((paging.PageNumber - 1) * paging.PageSize)
            .Take(paging.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<User>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = paging.PageNumber,
            PageSize = paging.PageSize
        };
    }

    private IQueryable<User> ApplyUserFilters(IQueryable<User> query, UserFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string searchTermLower = filter.SearchTerm.ToLower();
            query = query.Where(u => u.UserName.ToLower().Contains(searchTermLower) ||
                                     u.Email.ToLower().Contains(searchTermLower));
        }

        if (!string.IsNullOrWhiteSpace(filter.Role))
        {
            string roleLower = filter.Role.ToLower();
            query = query.Where(u => u.Roles.Any(r => r.Name.ToLower() == roleLower));
        }

        return query;
    }

    private IQueryable<User> ApplyUserSorting(IQueryable<User> query, Sorting sorting)
    {
        string? normalizedSortOrder = sorting.SortOrder?.ToLower();
        bool isDescending = normalizedSortOrder == "desc";

        if (string.IsNullOrWhiteSpace(sorting.SortBy))
        {
            return isDescending ? query.OrderByDescending(u => u.CreatedDate) : query.OrderBy(u => u.CreatedDate);
        }

        string sortToLower = sorting.SortBy.ToLower();
        switch (sortToLower)
        {
            case "username":
                query = isDescending ? query.OrderByDescending(u => u.UserName) : query.OrderBy(u => u.UserName);
                break;
            case "email":
                query = isDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email);
                break;
            case "createddate":
                query = isDescending ? query.OrderByDescending(u => u.CreatedDate) : query.OrderBy(u => u.CreatedDate);
                break;
            default:
                query = isDescending ? query.OrderByDescending(u => u.CreatedDate) : query.OrderBy(u => u.CreatedDate);
                break;
        }
        return query;
    }

}
