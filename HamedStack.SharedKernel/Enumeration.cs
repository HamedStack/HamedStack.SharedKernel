// ReSharper disable UnusedMember.Global

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace HamedStack.SharedKernel;

/// <summary>
/// Provides a base class for creating an enumeration-like class with named instances.
/// This serves as a safer alternative to regular enums, providing more flexibility and ensuring type safety.
/// </summary>
/// <example>
/// An example of creating and using a derived enumeration:
/// <code>
/// public class Color : Enumeration
/// {
///     public static readonly Color Red = new Color(1, "Red");
///     public static readonly Color Blue = new Color(2, "Blue", "The Blue Color");
///     public static readonly Color Green = new Color(3, "Green");
///
///     public Color(int id, string name, string? description = default) : base(id, name, description) { }
/// }
///
/// var blueColor = Color.FromDisplayName("Blue");
/// var isRed = blueColor.Equals(Color.Red);
/// </code>
/// </example>
public abstract class Enumeration : IComparable
{
    private static readonly HashSet<string> ExistingNames = new();
    private static readonly HashSet<long> ExistingIds = new();
    private static readonly ConcurrentDictionary<Type, List<Enumeration>> Cache = new();
    private long _id;
    private string? _name;
    private string? _description;

    /// <summary>
    /// Event triggered when an enumeration property is read.
    /// </summary>
    public event EventHandler<EnumerationReadEventArgs>? EnumerationRead;

    /// <summary>
    /// Event triggered when an enumeration property is written to.
    /// </summary>
    public event EventHandler<EnumerationWriteEventArgs>? EnumerationWrite;

    /// <summary>
    /// Gets or sets the ID of the enumeration item.
    /// </summary>
    public long Id
    {
        get
        {
            OnEnumerationRead(nameof(Id));
            return _id;
        }
        protected set
        {
            OnEnumerationWrite(nameof(Id), value);
            _id = value;
        }
    }

    /// <summary>
    /// Gets or sets the name of the enumeration item.
    /// </summary>
    public string Name
    {
        get
        {
            OnEnumerationRead(nameof(Name));
            return _name!;
        }
        protected set
        {
            OnEnumerationWrite(nameof(Name), value);
            _name = value;
        }
    }

    /// <summary>
    /// Gets or sets the description of the enumeration item.
    /// </summary>
    public string Description
    {
        get
        {
            OnEnumerationRead(nameof(Description));
            return _description ?? string.Empty;
        }
        protected set
        {
            OnEnumerationWrite(nameof(Description), value);
            _description = value;
        }
    }

    /// <summary>
    /// Raises the <see cref="EnumerationRead"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the accessed property.</param>
    protected virtual void OnEnumerationRead(string propertyName)
    {
        EnumerationRead?.Invoke(this, new EnumerationReadEventArgs(propertyName));
    }

    /// <summary>
    /// Raises the <see cref="EnumerationWrite"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property being written to.</param>
    /// <param name="value">The new value being assigned.</param>
    protected virtual void OnEnumerationWrite(string propertyName, object value)
    {
        EnumerationWrite?.Invoke(this, new EnumerationWriteEventArgs(propertyName, value));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Enumeration"/> class with the provided identifier, name, and description.
    /// </summary>
    /// <param name="id">The unique identifier for the enumeration instance. This should be non-negative and unique across all instances of the enumeration.</param>
    /// <param name="name">The name of the enumeration instance. This should be unique across all instances of the enumeration and cannot be null or empty.</param>
    /// <param name="description">An optional description for the enumeration instance. If not provided or null, defaults to an empty string.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="name"/> is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="id"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the <paramref name="name"/> or <paramref name="id"/> is already used in another instance of the enumeration.</exception>
    protected Enumeration(int id, string name, string? description = "")
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");

        if (id < 0)
            throw new ArgumentException("ID cannot be negative.", nameof(id));

        if (!ExistingNames.Add(name))
            throw new InvalidOperationException($"The name '{name}' is already used.");

        if (!ExistingIds.Add(id))
            throw new InvalidOperationException($"The ID '{id}' is already used.");

        (Id, Name, Description) = (id, name, description ?? string.Empty);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Enumeration"/> class with the provided identifier, name, and description.
    /// </summary>
    /// <param name="id">The unique identifier for the enumeration instance. This should be non-negative and unique across all instances of the enumeration.</param>
    /// <param name="name">The name of the enumeration instance. This should be unique across all instances of the enumeration and cannot be null or empty.</param>
    /// <param name="description">An optional description for the enumeration instance. If not provided or null, defaults to an empty string.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="name"/> is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="id"/> is negative.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the <paramref name="name"/> or <paramref name="id"/> is already used in another instance of the enumeration.</exception>
    protected Enumeration(long id, string name, string? description = "")
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");

        if (id < 0)
            throw new ArgumentException("ID cannot be negative.", nameof(id));

        if (!ExistingNames.Add(name))
            throw new InvalidOperationException($"The name '{name}' is already used.");

        if (!ExistingIds.Add(id))
            throw new InvalidOperationException($"The ID '{id}' is already used.");

        (Id, Name, Description) = (id, name, description ?? string.Empty);
    }

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <summary>
    /// Retrieves all instances of a specified enumeration type.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <returns>An enumerable collection of enumeration instances.</returns>
    /// <remarks>
    /// This method caches the results per type for efficiency.
    /// </remarks>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        if (Cache.TryGetValue(typeof(T), out var values))
            return values.Cast<T>();

        var items = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(f => f.FieldType == typeof(T))
            .Select(f => f.GetValue(null))
            .Cast<T>()
            .ToList();

        Cache[typeof(T)] = items.OfType<Enumeration>().ToList();

        return items;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = GetType() == obj.GetType();
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    /// <inheritdoc />
    public override int GetHashCode() => Id.GetHashCode();

    /// <summary>
    /// Calculates the absolute difference between two <see cref="Enumeration"/> instances based on their IDs.
    /// </summary>
    /// <param name="firstValue">The first enumeration value.</param>
    /// <param name="secondValue">The second enumeration value.</param>
    /// <returns>The absolute difference between the two enumeration IDs.</returns>
    public static long AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
    {
        var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
        return absoluteDifference;
    }
    /// <summary>
    /// Parses and retrieves an instance of <typeparamref name="T"/> based on the provided predicate.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <param name="predicate">The predicate used to match the enumeration instance.</param>
    /// <returns>The matched enumeration instance.</returns>
    private static T Parse<T>(Func<T, bool> predicate) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);
        return matchingItem ?? throw new InvalidOperationException($"No matching enumeration value found in {typeof(T)}");
    }

    /// <summary>
    /// Retrieves an instance of <typeparamref name="T"/> based on the provided value.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <param name="value">The ID value to match against.</param>
    /// <returns>The matched enumeration instance.</returns>
    public static T FromValue<T>(int value) where T : Enumeration
    {
        return Parse<T>(item => item.Id == value);
    }

    /// <summary>
    /// Retrieves an instance of <typeparamref name="T"/> based on the provided display name.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <param name="displayName">The name to match against.</param>
    /// <returns>The matched enumeration instance.</returns>
    public static T FromDisplayName<T>(string displayName) where T : Enumeration
    {
        return Parse<T>(item => item.Name == displayName);
    }

    /// <summary>
    /// Provides a collection of all enumeration values for <see cref="Enumeration"/>.
    /// </summary>
    public static IEnumerable<Enumeration> Values => GetAll<Enumeration>();

    /// <summary>
    /// Attempts to retrieve an instance of <typeparamref name="T"/> based on the provided ID value.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <param name="value">The ID value to match against.</param>
    /// <param name="result">If successful, contains the matched enumeration instance. Otherwise, contains null.</param>
    /// <returns><c>true</c> if a matching enumeration value was found; otherwise, <c>false</c>.</returns>
    public static bool TryFromValue<T>(int value, [NotNullWhen(true)] out T? result) where T : Enumeration
    {
        result = GetAll<T>().FirstOrDefault(item => item.Id == value);
        return result is not null;
    }

    /// <summary>
    /// Attempts to retrieve an instance of <typeparamref name="T"/> based on the provided display name.
    /// </summary>
    /// <typeparam name="T">The type of enumeration.</typeparam>
    /// <param name="displayName">The name to match against.</param>
    /// <param name="result">If successful, contains the matched enumeration instance. Otherwise, contains null.</param>
    /// <returns><c>true</c> if a matching enumeration value was found; otherwise, <c>false</c>.</returns>
    public static bool TryFromDisplayName<T>(string displayName, [NotNullWhen(true)] out T? result) where T : Enumeration
    {
        result = GetAll<T>().FirstOrDefault(item => item.Name == displayName);
        return result is not null;
    }

    /// <inheritdoc />
    public int CompareTo(object? other)
    {
        return other switch
        {
            null => 1,
            _ => other is not Enumeration otherEnumeration
                ? throw new ArgumentException("Comparing Enumeration type with different type is not supported.")
                : Id.CompareTo(otherEnumeration.Id)
        };
    }

    /// <summary>
    /// Compares the current instance with another instance of the same <see cref="Enumeration"/> type 
    /// and returns an integer that indicates whether the current instance precedes, 
    /// follows, or occurs in the same position in the sort order as the other instance.
    /// </summary>
    /// <param name="other">An instance of <see cref="Enumeration"/> to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.</returns>
    public int CompareTo(Enumeration? other)
    {
        return other is null ? 1 : Id.CompareTo(other.Id);
    }

    /// <summary>
    /// Determines if the <paramref name="left"/> <see cref="Enumeration"/> instance is less than the <paramref name="right"/> instance, based on their IDs.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns><c>true</c> if the ID of the <paramref name="left"/> operand is less than the ID of the <paramref name="right"/> operand; otherwise, <c>false</c>.</returns>
    public static bool operator <(Enumeration? left, Enumeration? right)
        => left is null ? right is not null : left.Id < right?.Id;

    /// <summary>
    /// Determines if the <paramref name="left"/> <see cref="Enumeration"/> instance is greater than the <paramref name="right"/> instance, based on their IDs.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns><c>true</c> if the ID of the <paramref name="left"/> operand is greater than the ID of the <paramref name="right"/> operand; otherwise, <c>false</c>.</returns>
    public static bool operator >(Enumeration? left, Enumeration? right)
        => left is not null && (right is null || left.Id > right.Id);

    /// <summary>
    /// Determines if the <paramref name="left"/> <see cref="Enumeration"/> instance is less than or equal to the <paramref name="right"/> instance, based on their IDs.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns><c>true</c> if the ID of the <paramref name="left"/> operand is less than or equal to the ID of the <paramref name="right"/> operand; otherwise, <c>false</c>.</returns>
    public static bool operator <=(Enumeration? left, Enumeration? right)
        => left is null || left.Id <= right?.Id;

    /// <summary>
    /// Determines if the <paramref name="left"/> <see cref="Enumeration"/> instance is greater than or equal to the <paramref name="right"/> instance, based on their IDs.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns><c>true</c> if the ID of the <paramref name="left"/> operand is greater than or equal to the ID of the <paramref name="right"/> operand; otherwise, <c>false</c>.</returns>
    public static bool operator >=(Enumeration? left, Enumeration? right)
        => right is null || left is not null && left.Id >= right.Id;

    /// <summary>
    /// Determines if the <paramref name="left"/> <see cref="Enumeration"/> instance is equal to the <paramref name="right"/> instance, based on their IDs.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns><c>true</c> if the ID of the <paramref name="left"/> operand is equal to the ID of the <paramref name="right"/> operand; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Enumeration? left, Enumeration? right)
        => left?.Equals(right) ?? right is null;

    /// <summary>
    /// Determines if the <paramref name="left"/> <see cref="Enumeration"/> instance is not equal to the <paramref name="right"/> instance, based on their IDs.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns><c>true</c> if the ID of the <paramref name="left"/> operand is not equal to the ID of the <paramref name="right"/> operand; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Enumeration? left, Enumeration? right)
        => !(left == right);
}