using System.ComponentModel.DataAnnotations;

namespace FitShirt.Domain.Purchasing.Models.Commands;

public class CreatePurchaseCommand
{
    [Required(ErrorMessage = "The user Id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "The number field must be a positive integer")]
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "Items list is required")]
    public List<CreateItemCommand> Items { get; set; }
}