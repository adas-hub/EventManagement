using EventManagement.Domain.Common;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Registrations.ApproveRegistration;

public class ApproveRegistrationCommandHandler : IRequestHandler<ApproveRegistrationCommand, Result>
{
    private readonly IRegistrationRepository _registrationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveRegistrationCommandHandler(IRegistrationRepository registrationRepository, IUnitOfWork unitOfWork)
    {
        _registrationRepository = registrationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ApproveRegistrationCommand request, CancellationToken cancellationToken)
    {
        var registration = await _registrationRepository.GetByIdAsync(request.RegistrationId, cancellationToken);

        if (registration == null)
        {
            return Result.Failure(Error.Failure("Registration.NotFound", "Registration not found."));
        }

        if (registration.Status == RegistrationStatus.Confirmed)
        {
            return Result.Failure(Error.Failure("Registration.AlreadyConfirmed", "Registration is already confirmed."));
        }
        if (registration.Status == RegistrationStatus.Rejected || registration.Status == RegistrationStatus.Cancelled)
        {
            return Result.Failure(Error.Failure("Registration.InvalidStatus", $"Cannot approve a registration with status: {registration.Status}."));
        }

        registration.Status = RegistrationStatus.Confirmed;
        registration.ApprovedDate = DateTime.UtcNow;
        registration.ApprovedByUserId = request.ApprovingUserId;

        _registrationRepository.Update(registration);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}