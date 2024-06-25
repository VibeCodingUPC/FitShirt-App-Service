namespace FitShirt.Application.Shared.Exceptions;

public class NotFoundEntityAttributeException : NotFoundException
{
    public string EntityName { get; }
    public string AttributeName { get; }
    public object AttributeValue { get; }
    
    public NotFoundEntityAttributeException(string entityName, string attributeName, object attributeValue)
        : base($"Entity '{entityName}' with attribute ({attributeName}: {attributeValue}) was not found")
    {
        EntityName = entityName;
        AttributeName = attributeName;
        AttributeValue = attributeValue;
    }
}