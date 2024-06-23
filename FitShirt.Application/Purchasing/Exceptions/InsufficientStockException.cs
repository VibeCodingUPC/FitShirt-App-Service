using FitShirt.Application.Shared.Exceptions;

namespace FitShirt.Application.Purchasing.Exceptions;

public class InsufficientStockException : ConflictException
{
    public InsufficientStockException() 
        : base("More quantity required than current stock")
    {
    }
}