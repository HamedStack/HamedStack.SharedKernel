// ReSharper disable UnusedMember.Global

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Defines a repository pattern, providing basic CRUD operations.
/// </summary>
public interface IRepository
{
    /// <summary>
    /// Gets the unit of work associated with the repository, allowing for transactional operations.
    /// </summary>
    IUnitOfWork UnitOfWork { get; }
}