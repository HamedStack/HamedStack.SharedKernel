// ReSharper disable UnusedMember.Global

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Defines a complete repository pattern for a specific type of entity, offering CRUD and read operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to work with.</typeparam>
public interface ICrudRepository<TEntity> : IReadRepository<TEntity>, IRepository
    where TEntity : class, IAggregateRoot
{
    /// <summary>
    /// Asynchronously adds a new entity.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously deletes an entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
}