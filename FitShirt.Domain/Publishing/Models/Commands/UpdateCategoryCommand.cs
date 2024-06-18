using System.ComponentModel.DataAnnotations;

namespace FitShirt.Domain.Publishing.Models.Commands;

public class UpdateCategoryCommand
{
    [Required(ErrorMessage = "Name field is required")] 
    [MaxLength(32)] 
    public string Name { get; set; }
}