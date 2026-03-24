using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Domain.Common;

public abstract class BaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Store events here temporarily
    //private readonly List<IDomainEvent> _domainEvents = new();
    
     // Tell the database not to store this list
    //public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    //public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    //public void ClearDomainEvents() => _domainEvents.Clear();
}