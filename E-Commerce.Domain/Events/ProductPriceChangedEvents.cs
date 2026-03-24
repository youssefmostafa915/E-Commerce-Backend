using E_Commerce.Domain.ValueObjects;

namespace E_Commerce.Domain.Events;

public record ProductPriceChangedEvent(
    string ProductId, 
    Money OldPrice, 
    Money NewPrice) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}