using System.ComponentModel.DataAnnotations;

namespace HospitalApi.DTOs;

public class BedAssignmentDto
{
    [Required]
    public DateTime From { get; set; }

    // opcjonalne - Example 2 nie zawiera "to"
    public DateTime? To { get; set; }

    [Required]
    public string BedType { get; set; } = null!;

    [Required]
    public string Ward { get; set; } = null!;
}
