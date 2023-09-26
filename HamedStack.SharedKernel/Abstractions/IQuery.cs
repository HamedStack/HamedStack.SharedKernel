// ReSharper disable UnusedTypeParameter

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Represents a query with a response of type TResponse.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IQuery<out TResponse>
{
}