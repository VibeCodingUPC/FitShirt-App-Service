using FitShirt.Application.Shared.Exceptions;

namespace FitShirt.Application.Security.Exceptions;

public class DuplicatedUserCellphoneException : DuplicatedEntityAttributeException
{
    public DuplicatedUserCellphoneException(object attributeValue) 
        : base("User", "Cellphone", attributeValue)
    {
    }
}