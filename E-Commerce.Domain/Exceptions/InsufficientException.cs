namespace E_Commerce.Domain.Exceptions;
public class InsufficientStockException : DomainException
{
    public InsufficientStockException(string productName) 
        : base($"Product '{productName}' does not have enough stock.") { }
}