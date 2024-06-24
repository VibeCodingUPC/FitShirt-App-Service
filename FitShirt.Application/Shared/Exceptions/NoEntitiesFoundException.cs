namespace FitShirt.Application.Shared.Exceptions;

public class NoEntitiesFoundException : NotFoundException
{
    public object EntityName { get; }
    
    public NoEntitiesFoundException(object entityName)
        : base($"No '{entityName}' found")
    {
        EntityName = entityName;
    }
        
}