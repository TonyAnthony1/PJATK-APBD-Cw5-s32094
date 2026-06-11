using HospitalApi.DTO;

namespace HospitalApi.Services;

public interface IPatientService
{
    Task<List<PatientGetDto>> GetPatientsAsync(string? search, CancellationToken cancellationToken);
}
