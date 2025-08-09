using EventManagement.Domain.Common;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Registrations.RejectRegistration;

public class RejectRegistrationCommandHandler : IRequestHandler<RejectRegistrationCommand, Result>
{
    private readonly IRegistrationRepository _registrationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectRegistrationCommandHandler(IRegistrationRepository registrationRepository, IUnitOfWork unitOfWork)
    {
        _registrationRepository = registrationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RejectRegistrationCommand request, CancellationToken cancellationToken)
    {
        // 1. Get the registration
        var registration = await _registrationRepository.GetByIdAsync(request.RegistrationId, cancellationToken);

        if (registration == null)
        {
            return Result.Failure(Error.Failure("Registration.NotFound", "Registration not found."));
        }

        if (registration.Status == RegistrationStatus.Rejected)
        {
            return Result.Failure(Error.Failure("Registration.AlreadyRejected", "Registration is already rejected."));
        }
        if (registration.Status == RegistrationStatus.Confirmed || registration.Status == RegistrationStatus.Cancelled)
        {
            return Result.Failure(Error.Failure("Registration.InvalidStatus", $"Cannot reject a registration with status: {registration.Status}."));
        }

        registration.Status = RegistrationStatus.Rejected;
        registration.RejectedDate = DateTime.UtcNow;
        registration.RejectedByUserId = request.RejectingUserId;
        registration.RejectionReason = request.RejectionReason;

        _registrationRepository.Update(registration);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}