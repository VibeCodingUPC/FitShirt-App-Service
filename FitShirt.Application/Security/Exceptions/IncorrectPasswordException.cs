using System.ComponentModel.DataAnnotations;

namespace FitShirt.Application.Security.Exceptions;

public class IncorrectPasswordException : ValidationException
{
    public IncorrectPasswordException() : base("Incorrect password")
    {
    }
}