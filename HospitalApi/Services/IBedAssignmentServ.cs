using HospitalApi.DTO;

namespace HospitalApi.Services;

public interface IBedAssignmentServ
{
    Task<int> AssignBedAsync(string pesel, CreateBedAssignmentDto request, CancellationToken cancellationToken);
}
