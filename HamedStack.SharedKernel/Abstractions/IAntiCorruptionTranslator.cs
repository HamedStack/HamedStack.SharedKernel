// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

namespace HamedStack.SharedKernel.Abstractions;

/// <summary>
/// Defines a contract to translate a source object to a destination object.
/// </summary>
/// <typeparam name="TSource">The type of the source object.</typeparam>
/// <typeparam name="TDestination">The type of the destination object.</typeparam>
public interface IAntiCorruptionTranslator<in TSource, out TDestination>
{
    /// <summary>
    /// Translates the specified source object to its corresponding destination object.
    /// </summary>
    /// <param name="source">The source object to be translated.</param>
    /// <returns>The translated destination object.</returns>
    TDestination Translate(TSource source);
}