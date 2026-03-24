namespace E_Commerce.Application.DTOs.Products;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PriceAmount { get; set; }   
    public string Currency { get; set; } = "USD";
    public int StockQuantity { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } =new();
}