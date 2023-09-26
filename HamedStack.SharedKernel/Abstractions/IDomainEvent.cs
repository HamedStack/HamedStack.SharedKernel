// ReSharper disable UnusedMember.Global

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Defines the contract for domain events in the system.
/// </summary>
/// <remarks>
/// Domain events are instances of events that result from domain actions, 
/// such as entity creation, updates, or any business-specific actions. 
/// Implementing this interface ensures that an event captures the exact 
/// moment it occurred. This can be useful for event-driven architectures, 
/// logging, auditing, or event sourcing.
/// </remarks>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the date and time (with offset) when the domain event occurred.
    /// </summary>
    /// <value>
    /// The date and time of the event.
    /// </value>
    /// <remarks>
    /// This property provides a precise timestamp that helps in ordering events 
    /// and understanding the sequence of domain changes.
    /// </remarks>
    DateTimeOffset OccurredOn { get; }
}