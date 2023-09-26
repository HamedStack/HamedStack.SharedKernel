namespace HamedStack.SharedKernel;

/// <summary>
/// Provides data for the <see cref="Enumeration.EnumerationRead"/> event.
/// </summary>
public class EnumerationReadEventArgs : EventArgs
{
    /// <summary>
    /// Gets the name of the accessed property.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumerationReadEventArgs"/> class.
    /// </summary>
    /// <param name="propertyName">The name of the accessed property.</param>
    public EnumerationReadEventArgs(string propertyName)
    {
        PropertyName = propertyName;
    }
}