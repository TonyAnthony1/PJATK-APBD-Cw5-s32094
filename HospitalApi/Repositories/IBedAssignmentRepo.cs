using HospitalApi.Models;

namespace HospitalApi.Repositories;

public interface IBedAssignmentRepo
{
    Task<BedType?> GetBedTypeByNameAsync(string name, CancellationToken cancellationToken);
    Task<Ward?> GetWardByNameAsync(string name, CancellationToken cancellationToken);

    
    Task<Bed?> FindFreeBedAsync(int bedTypeId, int wardId, DateTime from, DateTime? to, CancellationToken cancellationToken);

    Task AddBedAssignmentAsync(BedAssignment assignment, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
