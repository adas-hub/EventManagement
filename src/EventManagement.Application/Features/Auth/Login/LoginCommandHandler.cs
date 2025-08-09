using EventManagement.Application.DTOs.Auth;
using EventManagement.Application.Interfaces;
using EventManagement.Domain.Common;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            return Error.Failure("Auth.UserNotFound", $"User with email '{request.Email}' not found.");
        }

        bool isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return Error.Failure("Auth.InvalidCredentials", "Invalid email or password.");
        }

        var token = _jwtService.GenerateToken(user);
        var authResponse = new AuthResponseDto
        {
            Token = token,
            UserName = user.UserName
        };

        return authResponse;
    }
}