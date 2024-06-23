using System.ComponentModel.DataAnnotations;

namespace FitShirt.Domain.Purchasing.Models.Commands;

public class DeletePurchaseCommand
{
    [Required]
    public int Id { get; set; }
}