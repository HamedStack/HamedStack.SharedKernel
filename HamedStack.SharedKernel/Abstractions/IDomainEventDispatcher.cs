namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Defines a mechanism for dispatching domain events.
/// </summary>
/// <remarks>
/// Implementors of this interface are responsible for determining how domain events
/// are dispatched and handled. This could involve publishing the events to external systems,
/// handling them within the current application context, or any other desired behavior.
/// </remarks>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches the provided domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to be dispatched.</param>
    /// <remarks>
    /// Implementors should handle the dispatching logic, which might include 
    /// publishing the event to a message bus, invoking event handlers, or any other 
    /// mechanism suitable for the application's requirements.
    /// </remarks>
    void Dispatch(IDomainEvent domainEvent);
}