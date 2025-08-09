using EventManagement.Application.Interfaces;
using EventManagement.Domain.Common;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Auth.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            return Error.Failure("User.EmailAlreadyRegistered.Conflict", $"A user with email '{request.Email}' is already registered.");
        }

        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        var newUser = new User
        {
            Email = request.Email,
            UserName = request.UserName,
            PasswordHash = hashedPassword,
        };

        var defaultRole = await _roleRepository.GetRoleByNameAsync(UserRoles.User.ToString());
        if (defaultRole == null)
        {
            return Error.Failure("Role.NotFound", $"Default role '{UserRoles.User}' not found.");
        }
        newUser.Roles.Add(defaultRole);

        await _userRepository.AddAsync(newUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}