namespace FitShirt.Application.Shared.Exceptions;

public class NotFoundEntityIdException : NotFoundEntityAttributeException
{
    public NotFoundEntityIdException(string entityName, object attributeValue) 
        : base(entityName, "Id", attributeValue)
    {
    }
}