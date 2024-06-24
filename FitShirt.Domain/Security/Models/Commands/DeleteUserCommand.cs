using System.ComponentModel.DataAnnotations;

namespace FitShirt.Domain.Security.Models.Commands;

public class DeleteUserCommand
{
    [Required]
    public int Id { get; set; }
}