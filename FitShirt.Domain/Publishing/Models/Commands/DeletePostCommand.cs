using System.ComponentModel.DataAnnotations;

namespace FitShirt.Domain.Publishing.Models.Commands;

public class DeletePostCommand
{
    [Required]
    public int Id { get; set; }
}