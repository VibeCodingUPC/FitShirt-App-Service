using FitShirt.Application.Shared.Exceptions;

namespace FitShirt.Application.Security.Exceptions;

public class DuplicatedUserEmailException : DuplicatedEntityAttributeException
{
    public DuplicatedUserEmailException(object attributeValue) 
        : base("User", "Email", attributeValue)
    {
    }
}