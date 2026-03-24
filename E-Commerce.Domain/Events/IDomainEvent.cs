using MediatR;

namespace E_Commerce.Domain.Events;

public interface IDomainEvent : INotification
{
    // The exact UTC time the event happened
    DateTime OccurredOn { get; }
}