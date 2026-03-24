namespace E_Commerce.Domain.Entities;

public class Cart : BaseEntity
{
    public string? UserId { get; set; } 
    public string GuestId { get; set; } = string.Empty;
    public List<CartItem> Items { get; private set; } = new();

    public void AddOrUpdateItem(CartItem item)
    {
        var existing = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
        if (existing != null)
        {
            Items.Remove(existing);
            // 'with' keyword creates a copy of the record with a new quantity
            Items.Add(existing with { Quantity = existing.Quantity + item.Quantity });
        }
        else
        {
            Items.Add(item);
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public void ClearCart() => Items.Clear();
}