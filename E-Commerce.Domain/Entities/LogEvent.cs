namespace E_Commerce.Domain.Entities;

public class LogEvent : BaseEntity
{
    public string EventType { get; set; } = string.Empty; // e.g., "ProductView"
    public string? UserId { get; set; }
    public string? GuestId { get; set; }
    public string? ProductId { get; set; }
    
    // The flexible Metadata dictionary for extra info
    public Dictionary<string, object> Metadata { get; set; } = new();

    public void AddDetail(string key, object value) => Metadata[key] = value;
}