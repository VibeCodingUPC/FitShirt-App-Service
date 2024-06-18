using System.ComponentModel.DataAnnotations;

namespace FitShirt.Domain.Publishing.Models.Commands;

public class DeleteCategoryCommand
{
    [Required]
    public int Id { get; set; }
}