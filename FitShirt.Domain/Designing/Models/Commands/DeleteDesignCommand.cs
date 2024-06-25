using System.ComponentModel.DataAnnotations;

namespace FitShirt.Domain.Designing.Models.Commands;

public class DeleteDesignCommand
{
    [Required]
    public int Id { get; set; }
}