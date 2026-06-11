using HospitalApi.Data;
using HospitalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalApi.Repositories;

public class BedAssignmentRepo : IBedAssignmentRepo
{
    private readonly HospitalContext _context;

    public BedAssignmentRepo(HospitalContext context)
    {
        _context = context;
    }

    public Task<BedType?> GetBedTypeByNameAsync(string name, CancellationToken cancellationToken)
    {
        return _context.BedTypes.FirstOrDefaultAsync(bt => bt.Name == name, cancellationToken);
    }

    public Task<Ward?> GetWardByNameAsync(string name, CancellationToken cancellationToken)
    {
        return _context.Wards.FirstOrDefaultAsync(w => w.Name == name, cancellationToken);
    }

    public Task<Bed?> FindFreeBedAsync(int bedTypeId, int wardId, DateTime from, DateTime? to, CancellationToken cancellationToken)
    {
        
        var candidates = _context.Beds
            .Where(b => b.BedTypeId == bedTypeId && b.Room.WardId == wardId);

        
        var effectiveTo = to ?? DateTime.MaxValue;

        var freeBed = candidates
            .Where(b => !b.BedAssignments.Any(ba =>
                ba.From < effectiveTo &&
                (ba.To == null || ba.To > from)))
            .OrderBy(b => b.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return freeBed;
    }

    public async Task AddBedAssignmentAsync(BedAssignment assignment, CancellationToken cancellationToken)
    {
        await _context.BedAssignments.AddAsync(assignment, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
