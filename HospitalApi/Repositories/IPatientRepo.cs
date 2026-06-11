using HospitalApi.Models;

namespace HospitalApi.Repositories;

public interface IPatientRepo
{
    
    Task<List<Patient>> GetPatientsAsync(string? search, CancellationToken cancellationToken);
    Task<bool> PatientExistsAsync(string pesel, CancellationToken cancellationToken);
}
