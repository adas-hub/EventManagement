using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Auth.RegisterUser;

public record class RegisterUserCommand(string Email, string UserName, string Password) : IRequest<Result>;
