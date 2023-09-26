// ReSharper disable UnusedTypeParameter

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Represents a command with a response of type TResponse.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface ICommand<out TResponse>
{
}