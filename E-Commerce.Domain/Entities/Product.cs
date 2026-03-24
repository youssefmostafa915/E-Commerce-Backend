namespace E_Commerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Money Price { get; set; } = new(0);
    public int StockQuantity { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = new();

    // Optimistic Concurrency version
    public int Version { get; set; } = 1;

    public void UpdateStock(int quantityChange)
    {
        if (StockQuantity + quantityChange < 0)
            throw new E_Commerce.Domain.Exceptions.InsufficientStockException(Name);

        StockQuantity += quantityChange;
        Version++;
        UpdatedAt = DateTime.UtcNow;
    }
}