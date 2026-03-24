namespace E_Commerce.Domain.Events;

public record UserRegisteredEvent(string UserId, string Email) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}