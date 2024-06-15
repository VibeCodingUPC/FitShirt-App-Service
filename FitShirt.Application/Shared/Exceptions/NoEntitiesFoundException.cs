namespace FitShirt.Application.Shared.Exceptions;

public class NoEntitiesFoundException : NotFoundException
{
    public NoEntitiesFoundException(object entityName)
        : base($"No '{entityName}' found")
    {
    }
        
}