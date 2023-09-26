// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Defines a contract for versioned entities using a byte array as the version type.
/// </summary>
/// <remarks>
/// This interface is a specialization of the generic <see cref="IVersioned{TVersion}"/> interface, 
/// standardizing on using a byte array as the versioning mechanism. It's commonly used for 
/// optimistic concurrency control, where the `RowVersion` represents a timestamp or version number 
/// that is updated with each change to the record.
/// </remarks>
public interface IVersioned : IVersioned<byte[]>
{
}

/// <summary>
/// Defines a contract for versioned entities with a custom version type.
/// </summary>
/// <typeparam name="TVersion">The type of the version identifier.</typeparam>
/// <remarks>
/// Versioning is a mechanism to handle optimistic concurrency in systems. Entities implementing 
/// this interface have a version field that gets updated every time there's a change. When updating 
/// an entity, systems can use this version value to ensure that no other operations have modified 
/// the entity since it was last fetched, preventing concurrency conflicts.
/// </remarks>
public interface IVersioned<TVersion>
{
    /// <summary>
    /// Gets or sets the version of the entity.
    /// </summary>
    /// <value>
    /// The version identifier for the entity.
    /// </value>
    /// <remarks>
    /// The version is typically a timestamp or a sequential number that gets 
    /// updated every time there's a change to the entity. It's used to ensure 
    /// that no other operations have modified the entity since it was last read.
    /// </remarks>
    TVersion Version { get; set; }
}
