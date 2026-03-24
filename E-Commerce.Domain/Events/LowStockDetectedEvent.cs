namespace E_Commerce.Domain.Events;

public record LowStockDetectedEvent(string ProductId, int RemainingStock) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}