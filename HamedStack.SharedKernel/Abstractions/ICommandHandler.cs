// ReSharper disable UnusedTypeParameter
// ReSharper disable UnusedMember.Global

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Represents a handler for commands.
/// </summary>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}