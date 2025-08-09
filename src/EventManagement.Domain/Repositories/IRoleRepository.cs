using EventManagement.Domain.Entities;

namespace EventManagement.Domain.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken = default);
}
