using System.ComponentModel.DataAnnotations;

namespace FitShirt.Domain.Designing.Models.Commands;

public class CreateDesignCommand
{
    [Required(ErrorMessage = "This field is required")] 
    [MaxLength(32)] 
    public string Name { get; set; } = null!;
    
    [Required(ErrorMessage = "This field is required")]
    [Range(1, int.MaxValue, ErrorMessage = "The number field must be a positive integer")]
    public int PrimaryColorId { get; set; }
    
    [Required(ErrorMessage = "This field is required")]
    [Range(1, int.MaxValue, ErrorMessage = "The number field must be a positive integer")]
    public int SecondaryColorId { get; set; }
    
    [Required(ErrorMessage = "This field is required")]
    [Range(1, int.MaxValue, ErrorMessage = "The number field must be a positive integer")]
    public int TertiaryColorId { get; set; }
 
    public String? imageUrl { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [Range(1, int.MaxValue, ErrorMessage = "The number field must be a positive integer")]
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "This field is required")]
    [Range(1, int.MaxValue, ErrorMessage = "The number field must be a positive integer")]
    public int ShieldId { get; set; }
}