namespace E_Commerce.Domain.Events;

public record OrderPlacedEvent(
    string OrderId, 
    string UserId, 
    decimal TotalAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}