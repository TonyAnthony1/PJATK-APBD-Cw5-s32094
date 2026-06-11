using HospitalApi.Data;
using HospitalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalApi.Repositories;

public class PatientRepo : IPatientRepo
{
    private readonly HospitalContext _context;

    public PatientRepo(HospitalContext context)
    {
        _context = context;
    }

    public async Task<List<Patient>> GetPatientsAsync(string? search, CancellationToken cancellationToken)
    {
        var query = _context.Patients
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search}%";
            
            query = query.Where(p =>
                EF.Functions.Like(p.FirstName, pattern) ||
                EF.Functions.Like(p.LastName, pattern));
        }

        return await query
            .Include(p => p.Admissions)
                .ThenInclude(a => a.Ward)
            .Include(p => p.BedAssignments)
                .ThenInclude(ba => ba.Bed)
                    .ThenInclude(b => b.BedType)
            .Include(p => p.BedAssignments)
                .ThenInclude(ba => ba.Bed)
                    .ThenInclude(b => b.Room)
                        .ThenInclude(r => r.Ward)
            .OrderBy(p => p.LastName)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> PatientExistsAsync(string pesel, CancellationToken cancellationToken)
    {
        return _context.Patients.AnyAsync(p => p.Pesel == pesel, cancellationToken);
    }
}
