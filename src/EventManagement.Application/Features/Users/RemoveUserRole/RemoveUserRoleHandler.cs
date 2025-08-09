using EventManagement.Domain.Common;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Users.RemoveUserRole;

public class RemoveUserRoleCommandHandler : IRequestHandler<RemoveUserRoleCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveUserRoleCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdWithRolesAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Error.Failure("User.NotFound", $"User with ID '{request.UserId}' was not found.");
        }

        var roleToRemove = user.Roles.FirstOrDefault(r => r.Name == request.RoleName);
        if (roleToRemove == null)
        {
            return Error.Failure("User.RoleNotAssigned", $"User with ID '{request.UserId}' does not have the role '{request.RoleName}'.");
        }

        user.Roles.Remove(roleToRemove);

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}