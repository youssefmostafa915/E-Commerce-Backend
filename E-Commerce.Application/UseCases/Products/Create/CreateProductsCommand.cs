using E_Commerce.Application.DTOs.Products; 
using E_Commerce.Domain.Enums;

namespace E_Commerce.Application.UseCases.Products.Create;

public class CreateProductCommand
{
    // 1. Add 'set' or 'init' so the JSON serializer can write to them
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PriceAmount { get; set; }
    public string Currency { get; set; } = "USD";
    public int StockQuantity { get; set; }
    public ProductOperationType OperationType { get; set; }

    // 2. Add an empty constructor for the Serializer
    public CreateProductCommand() { }

    // 3. Keep your logic constructor if needed, but ensure it's not blocking the API
    public CreateProductCommand(string name, decimal priceAmount, ProductOperationType operationType)
    {
        Name = name;
        PriceAmount = priceAmount;
        OperationType = operationType;
    }
}