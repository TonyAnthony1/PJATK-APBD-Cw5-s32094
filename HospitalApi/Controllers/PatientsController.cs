using HospitalApi.DTO;
using HospitalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly IBedAssignmentServ _bedAssignmentServ;

    public PatientsController(
        IPatientService patientService,
        IBedAssignmentServ bedAssignmentServ)
    {
        _patientService = patientService;
        _bedAssignmentServ = bedAssignmentServ;
    }

  
    [HttpGet]
    public async Task<ActionResult<List<PatientGetDto>>> GetPatients(
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        var patients = await _patientService.GetPatientsAsync(search, cancellationToken);
        return Ok(patients);
    }
    
    [HttpPost("{pesel}/bedassignments")]
    public async Task<IActionResult> AssignBed(
        string pesel,
        [FromBody] CreateBedAssignmentDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var id = await _bedAssignmentServ.AssignBedAsync(pesel, request, cancellationToken);
            return Created($"/api/bedassignments/{id}", new { id });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new {message = ex.Message});
        }
        catch (ValidationException ex)
        {
            return BadRequest(new {message = ex.Message});
        }
    }
}