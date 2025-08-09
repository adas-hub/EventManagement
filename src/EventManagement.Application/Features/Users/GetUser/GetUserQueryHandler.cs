using EventManagement.Application.DTOs.Users;
using EventManagement.Application.Mappers;
using EventManagement.Domain.Common;
using EventManagement.Domain.Repositories;
using MediatR;
namespace EventManagement.Application.Features.Users.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdWithRolesAndEventsAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Error.Failure("User.NotFound", $"User with ID '{request.UserId}' was not found.");
        }

        return Result<UserDto>.Success(user.ToUserDto());
    }
}
