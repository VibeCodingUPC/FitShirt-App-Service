using FitShirt.Application.Shared.Exceptions;

namespace FitShirt.Application.Security.Exceptions;

public class DuplicatedUserUsernameException : DuplicatedEntityAttributeException
{
    public DuplicatedUserUsernameException(object attributeValue) 
        : base("User", "Username", attributeValue)
    {
    }
}