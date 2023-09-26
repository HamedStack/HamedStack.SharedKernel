// ReSharper disable UnusedMember.Global
// ReSharper disable VirtualMemberCallInConstructor

using System.Text.Json;
using HamedStack.SharedKernel.Abstractions;

namespace HamedStack.SharedKernel;

/// <summary>
/// A base class for entities that source their changes from domain events.
/// </summary>
/// <typeparam name="TId">The type of the entity's identifier.</typeparam>
public abstract class EventSourcedEntity<TId> : IId<TId>
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly Dictionary<Type, Action<IDomainEvent>> _eventHandlers = new();

    /// <summary>
    /// Gets the identifier of the entity.
    /// </summary>
    public TId Id { get; }

    /// <summary>
    /// Gets the current version of the entity, indicating how many events have been applied.
    /// </summary>
    public int EntityVersion { get; private set; }

    /// <summary>
    /// Gets a read-only collection of domain events raised by the entity.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the EventSourcedEntity class.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    protected EventSourcedEntity(TId id)
    {
        Id = id;
        RegisterEventHandlers(_eventHandlers);
    }

    /// <summary>
    /// Raises a new domain event and applies it to the entity.
    /// </summary>
    /// <param name="event">The domain event to raise and apply.</param>
    protected void RaiseEvent(IDomainEvent @event)
    {
        if (@event == null) throw new ArgumentNullException(nameof(@event));

        _domainEvents.Add(@event);
        ApplyEvent(@event);
        EntityVersion++;
    }

    /// <summary>
    /// Internally applies the provided domain event to the entity.
    /// </summary>
    /// <param name="event">The domain event to apply.</param>
    private void ApplyEvent(IDomainEvent @event)
    {
        if (_eventHandlers.TryGetValue(@event.GetType(), out var eventHandler))
        {
            eventHandler(@event);
        }
        else
        {
            throw new InvalidOperationException($"No event handler registered for {@event.GetType().Name}");
        }
    }

    /// <summary>
    /// Abstract method to register event handlers. Derived entities should populate the provided dictionary with their event handlers.
    /// </summary>
    /// <param name="eventHandlers">The dictionary to populate with event handlers.</param>
    protected abstract void RegisterEventHandlers(Dictionary<Type, Action<IDomainEvent>> eventHandlers);

    /// <summary>
    /// Reconstructs the entity's state from a historical sequence of events.
    /// </summary>
    /// <param name="history">The sequence of historical events to apply.</param>
    public void LoadFromHistory(IEnumerable<IDomainEvent> history)
    {
        foreach (var e in history)
        {
            ApplyEvent(e);
            EntityVersion++;
        }
    }

    /// <summary>
    /// Restores the entity's state from a snapshot and a subsequent list of events.
    /// </summary>
    /// <param name="snapshotVersion">The version at which the snapshot was taken.</param>
    /// <param name="eventsAfterSnapshot">Events that occurred after the snapshot.</param>
    public void RestoreFromSnapshot(int snapshotVersion, IEnumerable<IDomainEvent> eventsAfterSnapshot)
    {
        EntityVersion = snapshotVersion;
        LoadFromHistory(eventsAfterSnapshot);
    }

    /// <summary>
    /// Creates a snapshot of the current state of the entity.
    /// Derived classes can override this method for custom serialization.
    /// </summary>
    /// <returns>A serialized representation of the current state of the entity.</returns>
    public virtual string CreateSnapshot()
    { 
        return JsonSerializer.Serialize(this);
    }

    /// <summary>
    /// Checks if the current version matches the expected version, used for concurrency checks.
    /// </summary>
    /// <param name="expectedVersion">The expected version of the entity.</param>
    public void CheckVersion(int expectedVersion)
    {
        if (EntityVersion != expectedVersion)
            throw new InvalidOperationException("Concurrency conflict.");
    }

    /// <summary>
    /// Clears all raised domain events from the entity.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object based on the type and ID.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not EventSourcedEntity<TId> other)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        if (Id.Equals(default(TId)) || other.Id.Equals(default(TId)))
            return false;

        return Id.Equals(other.Id);
    }

    /// <summary>
    /// Serves as the default hash function based on the entity's type and ID.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }

    /// <summary>
    /// Determines whether two entities are equal based on their IDs.
    /// </summary>
    /// <param name="a">The first entity to compare.</param>
    /// <param name="b">The second entity to compare.</param>
    /// <returns>True if the two entities have the same ID; otherwise, false.</returns>
    public static bool operator ==(EventSourcedEntity<TId>? a, EventSourcedEntity<TId>? b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;

        return a.Equals(b);
    }

    /// <summary>
    /// Determines whether two entities are not equal based on their IDs.
    /// </summary>
    /// <param name="a">The first entity to compare.</param>
    /// <param name="b">The second entity to compare.</param>
    /// <returns>True if the two entities do not have the same ID; otherwise, false.</returns>
    public static bool operator !=(EventSourcedEntity<TId>? a, EventSourcedEntity<TId>? b)
    {
        return !(a == b);
    }

}