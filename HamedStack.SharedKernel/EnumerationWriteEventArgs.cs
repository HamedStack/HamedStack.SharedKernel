namespace HamedStack.SharedKernel;

/// <summary>
/// Provides data for the <see cref="Enumeration.EnumerationWrite"/> event.
/// </summary>
public class EnumerationWriteEventArgs : EventArgs
{
    /// <summary>
    /// Gets the name of the property being written to.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the new value being assigned.
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumerationWriteEventArgs"/> class.
    /// </summary>
    /// <param name="propertyName">The name of the property being written to.</param>
    /// <param name="value">The new value being assigned.</param>
    public EnumerationWriteEventArgs(string propertyName, object value)
    {
        PropertyName = propertyName;
        Value = value;
    }
}