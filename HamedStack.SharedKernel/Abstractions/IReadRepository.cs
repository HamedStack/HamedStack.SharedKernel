// ReSharper disable UnusedMember.Global

using System.Linq.Expressions;

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Defines read operations on a repository for a specific type of entity.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to work with.</typeparam>
public interface IReadRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity found, or null.</returns>
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously retrieves all entities.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously retrieves entities based on specified filter criteria, ordering, and pagination parameters.
    /// </summary>
    /// <param name="filter">An expression to filter the entities.</param>
    /// <param name="orderBy">A function to order the entities. If null, no ordering is applied.</param>
    /// <param name="skip">The number of entities to skip. If null, no entities are skipped.</param>
    /// <param name="take">The number of entities to take. If null, all filtered entities are taken.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of filtered entities.</returns>
    Task<IEnumerable<TEntity>> GetByFilterAsync(
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves entities based on specified filter criteria, ordering, 
    /// pagination parameters, and included properties for eager loading.
    /// </summary>
    /// <param name="filter">An expression to filter the entities.</param>
    /// <param name="orderBy">A function to order the entities. If null, no ordering is applied.</param>
    /// <param name="skip">The number of entities to skip. If null, no entities are skipped.</param>
    /// <param name="take">The number of entities to take. If null, all filtered entities are taken.</param>
    /// <param name="includeProperties">Expressions for properties to be included for eager loading.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of filtered entities.</returns>
    Task<IEnumerable<TEntity>> GetByFilterWithIncludesAsync(
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int? skip = null,
        int? take = null,
        IEnumerable<Expression<Func<TEntity, object>>>? includeProperties = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves entities as an asynchronous stream based on a specified filter criteria.
    /// </summary>
    /// <param name="filter">An expression to filter the entities.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An asynchronous stream of filtered entities.</returns>
    IAsyncEnumerable<TEntity> GetAsyncEnumerable(Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously retrieves entities as an asynchronous stream based on a specified filter criteria and included properties.
    /// </summary>
    /// <param name="filter">An expression to filter the entities.</param>
    /// <param name="includeProperties">Expressions for properties to be included for eager loading.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An asynchronous stream of filtered entities.</returns>
    IAsyncEnumerable<TEntity> GetAsyncEnumerableWithIncludes(Expression<Func<TEntity, bool>> filter,
        IEnumerable<Expression<Func<TEntity, object>>>? includeProperties,
        CancellationToken cancellationToken);
}