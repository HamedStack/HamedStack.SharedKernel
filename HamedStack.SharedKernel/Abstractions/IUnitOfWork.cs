// ReSharper disable UnusedMember.Global

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Represents a unit of work pattern, offering methods for transactional operations.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Begins a new transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task BeginTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Commits the current transaction changes asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CommitTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Rolls back the current transaction changes asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
}