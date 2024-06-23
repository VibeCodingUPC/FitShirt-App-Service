using FitShirt.Application.Shared.Exceptions;

namespace FitShirt.Application.Security.Exceptions;

public class UserLowerAgeException : ValidationException
{
    public UserLowerAgeException() 
        : base("User must be, at least, 18 years old")
    {
    }
}