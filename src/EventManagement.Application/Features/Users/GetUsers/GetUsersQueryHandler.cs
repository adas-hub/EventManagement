using EventManagement.Application.DTOs.Users;
using EventManagement.Application.Mappers;
using EventManagement.Domain.Common;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Users.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<PagedResult<UserDto>>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<PagedResult<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var pagedUsers = await _userRepository.GetUsersAsync(
            request.Paging,
            request.Filter,
            request.Sorting,
            cancellationToken);

        var userDtos = pagedUsers.Items.Select(u => u.ToUserDto()).ToList();

        var result = new PagedResult<UserDto>
        {
            Items = userDtos,
            TotalCount = pagedUsers.TotalCount,
            PageNumber = pagedUsers.PageNumber,
            PageSize = pagedUsers.PageSize
        };

        return Result<PagedResult<UserDto>>.Success(result);
    }
}