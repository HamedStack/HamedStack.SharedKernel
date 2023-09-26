// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Provides a contract for entities that need to track creation and modification metadata.
/// </summary>
/// <remarks>
/// Implementing this interface ensures that an entity maintains an audit trail 
/// of its creation and any subsequent modifications. It's particularly useful 
/// for entities that need to adhere to regulatory compliance or simply provide
/// traceability of changes over time.
/// </remarks>
public interface IAuditable
{
    /// <summary>
    /// Gets or sets the date and time (with offset) when the entity was created.
    /// </summary>
    /// <value>
    /// The date and time of creation.
    /// </value>
    DateTimeOffset CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the user or process that created the entity.
    /// </summary>
    /// <value>
    /// The identifier for the user or process.
    /// </value>
    string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time (with offset) when the entity was last modified.
    /// </summary>
    /// <value>
    /// The date and time of the last modification or null if the entity hasn't been modified.
    /// </value>
    DateTimeOffset? ModifiedOn { get; set; }

    /// <summary>
    /// Gets or sets the user or process that last modified the entity.
    /// </summary>
    /// <value>
    /// The identifier for the user or process that made the last modification or null if the entity hasn't been modified.
    /// </value>
    string? ModifiedBy { get; set; }
}
