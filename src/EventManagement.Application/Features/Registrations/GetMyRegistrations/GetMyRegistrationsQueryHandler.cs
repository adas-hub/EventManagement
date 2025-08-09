using EventManagement.Application.DTOs.Registrations;
using EventManagement.Application.Mappers;
using EventManagement.Domain.Common;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Registrations.GetMyRegistrations;

public class GetMyRegistrationsQueryHandler : IRequestHandler<GetMyRegistrationsQuery, Result<PagedResult<RegistrationDto>>>
{
    private readonly IRegistrationRepository _registrationRepository;

    public GetMyRegistrationsQueryHandler(IRegistrationRepository registrationRepository)
    {
        _registrationRepository = registrationRepository;
    }

    public async Task<Result<PagedResult<RegistrationDto>>> Handle(GetMyRegistrationsQuery request, CancellationToken cancellationToken)
    {
        var pagedRegistrations = await _registrationRepository.GetUserRegistrationsAsync(
            request.UserId,
            request.Paging,
            request.Filter,
            request.Sorting,
            cancellationToken);

        var registrationDtos = pagedRegistrations.Items.Select(r => r.ToRegistrationDto()).ToList();

        var result = new PagedResult<RegistrationDto>
        {
            Items = registrationDtos,
            TotalCount = pagedRegistrations.TotalCount,
            PageNumber = pagedRegistrations.PageNumber,
            PageSize = pagedRegistrations.PageSize
        };

        return Result<PagedResult<RegistrationDto>>.Success(result);
    }
}