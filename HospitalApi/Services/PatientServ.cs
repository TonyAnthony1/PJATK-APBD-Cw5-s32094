using HospitalApi.DTO;
using HospitalApi.Models;
using HospitalApi.Repositories;

namespace HospitalApi.Services;

public class PatientServ : IPatientService
{
    private readonly IPatientRepo _patientRepository;

    public PatientServ(IPatientRepo patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<List<PatientGetDto>> GetPatientsAsync(string? search, CancellationToken cancellationToken)
    {
        var patients = await _patientRepository.GetPatientsAsync(search, cancellationToken);
        return patients.Select(MapPatient).ToList();
    }

    private static PatientGetDto MapPatient(Patient p) => new()
    {
        Pesel = p.Pesel,
        FirstName = p.FirstName,
        LastName = p.LastName,
        Age = p.Age,
        Sex = p.Sex ? "Male" : "Female", 
        Admissions = p.Admissions.Select(a => new AdmissionDto
        {
            Id = a.Id,
            AdmissionDate = a.AdmissionDate,
            DischargeDate = a.DischargeDate,
            Ward = new WardDto
            {
                Id = a.Ward.Id,
                Name = a.Ward.Name,
                Description = a.Ward.Description
            }
        }).ToList(),
        BedAssignments = p.BedAssignments.Select(ba => new BedAssignmentDto
        {
            Id = ba.Id,
            From = ba.From,
            To = ba.To,
            Bed = new BedDto
            {
                Id = ba.Bed.Id,
                BedType = new BedTypeDto
                {
                    Id = ba.Bed.BedType.Id,
                    Name = ba.Bed.BedType.Name,
                    Description = ba.Bed.BedType.Description
                },
                Room = new RoomDto
                {
                    Id = ba.Bed.Room.Id,
                    HasTv = ba.Bed.Room.HasTv,
                    Ward = new WardDto
                    {
                        Id = ba.Bed.Room.Ward.Id,
                        Name = ba.Bed.Room.Ward.Name,
                        Description = ba.Bed.Room.Ward.Description
                    }
                }
            }
        }).ToList()
    };
}
