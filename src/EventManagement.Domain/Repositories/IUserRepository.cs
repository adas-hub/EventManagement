using EventManagement.Domain.Common;
using EventManagement.Domain.Common.Filters;
using EventManagement.Domain.Entities;

namespace EventManagement.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByIdWithRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> GetByIdWithRolesAndEventsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<PagedResult<User>> GetUsersAsync(
        Paging paging,
        UserFilter filter,
        Sorting sorting,
        CancellationToken cancellationToken = default);
}
