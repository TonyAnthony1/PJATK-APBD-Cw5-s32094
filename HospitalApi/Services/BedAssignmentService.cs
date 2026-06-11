using HospitalApi.DTO;
using HospitalApi.Models;
using HospitalApi.Repositories;

namespace HospitalApi.Services;

public class BedAssignmentService : IBedAssignmentServ
{
    private readonly IBedAssignmentRepo _bedAssignmentRepository;
    private readonly IPatientRepo _patientRepository;

    public BedAssignmentService(
        IBedAssignmentRepo bedAssignmentRepository,
        IPatientRepo patientRepository)
    {
        _bedAssignmentRepository = bedAssignmentRepository;
        _patientRepository = patientRepository;
    }

    public async Task<int> AssignBedAsync(string pesel, CreateBedAssignmentDto request, CancellationToken cancellationToken)
    {
        if (request.To.HasValue && request.To.Value <= request.From)
        {
            throw new ValidationException("Field 'to' must be later than field 'from'.");
        }
        
        if (!await _patientRepository.PatientExistsAsync(pesel, cancellationToken))
        {
            throw new NotFoundException($"Patient with PESEL '{pesel}' was not found.");
        }

        var bedType = await _bedAssignmentRepository.GetBedTypeByNameAsync(request.BedType, cancellationToken);
        if (bedType is null)
        {
            throw new NotFoundException($"Bed type '{request.BedType}' was not found.");
        }

        var ward = await _bedAssignmentRepository.GetWardByNameAsync(request.Ward, cancellationToken);
        if (ward is null)
        {
            throw new NotFoundException($"Ward '{request.Ward}' was not found.");
        }

        var bed = await _bedAssignmentRepository.FindFreeBedAsync(
            bedType.Id, ward.Id, request.From, request.To, cancellationToken);

        if (bed is null)
        {
            throw new NotFoundException(
                $"No free bed of type '{request.BedType}' in ward '{request.Ward}' " +
                $"is available for the requested period.");
        }

        var assignment = new BedAssignment
        {
            PatientPesel = pesel,
            BedId = bed.Id,
            From = request.From,
            To = request.To
        };

        await _bedAssignmentRepository.AddBedAssignmentAsync(assignment, cancellationToken);
        await _bedAssignmentRepository.SaveChangesAsync(cancellationToken);

        return assignment.Id;
    }
}
