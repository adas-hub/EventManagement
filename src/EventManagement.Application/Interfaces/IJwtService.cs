using EventManagement.Domain.Entities;

namespace EventManagement.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
