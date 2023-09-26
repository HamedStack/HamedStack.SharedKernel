// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Defines a contract for an anti-corruption layer adapter to perform CRUD operations asynchronously.
/// </summary>
/// <typeparam name="TSource">The type of the source entity.</typeparam>
/// <typeparam name="TDestination">The type of the destination entity.</typeparam>
/// <typeparam name="TId">The type of the entity's identifier.</typeparam>
public interface IAntiCorruptionAdaptor<in TSource, TDestination, in TId>
{
    /// <summary>
    /// Retrieves an entity asynchronously based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to be retrieved.</param>
    /// <returns>The destination entity corresponding to the specified identifier.</returns>
    Task<TDestination> GetAsync(TId id);

    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>An enumerable collection of all destination entities.</returns>
    Task<IEnumerable<TDestination>> GetAllAsync();

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The source entity to be added.</param>
    /// <returns>The destination entity that has been added.</returns>
    Task<TDestination> AddAsync(TSource entity);

    /// <summary>
    /// Updates an existing entity based on its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity to be updated.</param>
    /// <param name="entity">The source entity with the updated information.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateAsync(TId id, TSource entity);

    /// <summary>
    /// Deletes an entity based on its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(TId id);
}

