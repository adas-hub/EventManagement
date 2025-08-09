using EventManagement.Application.DTOs.Auth;
using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponseDto>>;
