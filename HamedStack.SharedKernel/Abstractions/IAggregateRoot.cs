// ReSharper disable UnusedMember.Global

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Represents the root of an aggregate, a cluster of domain objects that are treated as a single unit 
/// for data changes. The Aggregate Root guarantees the consistency of changes being made within the aggregate 
/// by forbidding external objects from holding references to its members.
/// </summary>
public interface IAggregateRoot
{
    // This interface can be considered as a marker interface to identify aggregate roots.
}