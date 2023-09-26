// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedTypeParameter

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Represents a handler for queries.
/// </summary>
/// <typeparam name="TQuery">The type of the query.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}