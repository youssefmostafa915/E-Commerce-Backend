namespace E_Commerce.Domain.ValueObjects;

public record CartItem(
    string ProductId, 
    string ProductName, 
    decimal UnitPrice, 
    int Quantity, 
    string ImageUrl)
{
    public decimal TotalPrice => UnitPrice * Quantity;
}