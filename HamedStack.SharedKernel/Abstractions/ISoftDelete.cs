// ReSharper disable UnusedMember.Global

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Provides a contract for entities that support soft deletion.
/// </summary>
/// <remarks>
/// Soft deletion allows records to remain in the database but be flagged as "deleted", 
/// thus ensuring they're not included in normal application operations. Instead of 
/// removing the record from the database, a flag is set, indicating that the record 
/// is no longer active. This can be beneficial for maintaining historical data, audit trails, 
/// and allowing potential data recovery.
/// </remarks>
public interface ISoftDelete
{
    /// <summary>
    /// Gets or sets a value indicating whether the entity has been soft-deleted.
    /// </summary>
    /// <value>
    /// <c>true</c> if the entity is soft-deleted; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    /// When this property is set to <c>true</c>, the entity is considered as "deleted" 
    /// within the application's domain logic, even though it's still present in the database.
    /// </remarks>
    bool IsDeleted { get; set; }
}
