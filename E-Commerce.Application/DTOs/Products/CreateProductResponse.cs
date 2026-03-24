namespace E_Commerce.Application.DTOs.Products;

public class CreateProductResponse
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public int StockQuantity { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = new();

}



    