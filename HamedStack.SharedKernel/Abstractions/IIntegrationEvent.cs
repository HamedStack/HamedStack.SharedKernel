// ReSharper disable UnusedMember.Global

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Defines the contract for integration events with a <see cref="Guid"/> as the event identifier.
/// </summary>
/// <remarks>
/// Integration events are used to communicate changes across bounded contexts or microservices.
/// This interface is a specialization of the generic <see cref="IIntegrationEvent{TEventId}"/> interface,
/// standardizing on using a <see cref="Guid"/> as the event identifier for scenarios where 
/// a globally unique identifier is preferred.
/// </remarks>
public interface IIntegrationEvent : IIntegrationEvent<Guid>
{
}

/// <summary>
/// Defines the contract for integration events with a generic event identifier.
/// </summary>
/// <typeparam name="TEventId">The type of the event identifier.</typeparam>
/// <remarks>
/// Integration events are used to communicate changes across bounded contexts or microservices.
/// By making the event identifier generic, this interface offers flexibility in choosing
/// the type of identifier used for each specific integration event.
/// </remarks>
public interface IIntegrationEvent<out TEventId>
{
    /// <summary>
    /// Gets the date and time when the event occurred.
    /// </summary>
    /// <value>
    /// The date and time of the event.
    /// </value>
    DateTimeOffset OccurredOn { get; }

    /// <summary>
    /// Gets the unique identifier for the event.
    /// </summary>
    /// <value>
    /// The event's unique identifier.
    /// </value>
    /// <remarks>
    /// The type of the identifier is determined by the <typeparamref name="TEventId"/> type parameter.
    /// </remarks>
    TEventId EventId { get; }
}
