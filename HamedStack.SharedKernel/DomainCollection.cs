using System.Collections;

// ReSharper disable UnusedMember.Global

namespace HamedStack.SharedKernel;

/// <summary>
/// Represents a custom collection for domain entities.
/// </summary>
/// <typeparam name="T">The type of the elements in the collection.</typeparam>
public class DomainCollection<T> : ICollection<T>
{
    private readonly List<T> _list;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainCollection{T}"/> class.
    /// </summary>
    public DomainCollection()
    {
        _list = new List<T>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainCollection{T}"/> class with the provided items.
    /// </summary>
    /// <param name="items">The items to be added to the collection.</param>
    public DomainCollection(IEnumerable<T> items)
    {
        _list = items.ToList();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainCollection{T}"/> class with the provided array of items.
    /// </summary>
    /// <param name="items">The array of items to be added to the collection.</param>
    public DomainCollection(T[] items)
    {
        _list = new List<T>(items);
    }

    /// <summary>
    /// Gets the number of elements in the collection.
    /// </summary>
    public int Count => _list.Count;

    /// <summary>
    /// Gets a value indicating whether the collection is read-only. Always returns false.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The index of the element to retrieve.</param>
    /// <returns>The element at the specified index.</returns>
    public T this[int index] => _list[index];

    /// <summary>
    /// Adds an item to the collection.
    /// </summary>
    /// <param name="item">The item to be added.</param>
    public void Add(T item) => _list.Add(item);

    /// <summary>
    /// Adds a range of items to the collection.
    /// </summary>
    /// <param name="items">The items to be added.</param>
    public void AddRange(IEnumerable<T> items) => _list.AddRange(items);

    /// <summary>
    /// Clears the collection.
    /// </summary>
    public void Clear() => _list.Clear();

    /// <summary>
    /// Determines whether the collection contains a specific value.
    /// </summary>
    /// <param name="item">The item to locate in the collection.</param>
    /// <returns>true if item is found, otherwise false.</returns>
    public bool Contains(T item) => _list.Contains(item);

    /// <summary>
    /// Copies the elements of the collection to an array.
    /// </summary>
    /// <param name="array">The destination array.</param>
    /// <param name="arrayIndex">The index in the array at which copying begins.</param>
    public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    /// <summary>
    /// Removes the first occurrence of a specific item from the collection.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns>true if item was removed, otherwise false.</returns>
    public bool Remove(T item) => _list.Remove(item);

    /// <summary>
    /// Removes a range of items from the collection.
    /// </summary>
    /// <param name="items">The items to remove.</param>
    public void RemoveRange(IEnumerable<T> items) => _list.RemoveAll(items.Contains);

    /// <summary>
    /// Filters the collection based on a predicate.
    /// </summary>
    /// <param name="predicate">The predicate to use for filtering.</param>
    /// <returns>A collection of items that match the predicate.</returns>
    public IEnumerable<T> Where(Func<T, bool> predicate) => _list.Where(predicate);

    /// <summary>
    /// Sorts the collection.
    /// </summary>
    public void Sort() => _list.Sort();

    /// <summary>
    /// Sorts the collection using a specified comparer.
    /// </summary>
    /// <param name="comparer">The comparer to use for sorting.</param>
    public void Sort(IComparer<T> comparer) => _list.Sort(comparer);

    /// <summary>
    /// Finds an item in the collection that matches a specified predicate.
    /// </summary>
    /// <param name="match">The predicate to use for searching.</param>
    /// <returns>The first item that matches the predicate, or null if no match is found.</returns>
    public T? Find(Predicate<T> match) => _list.Find(match);

    /// <summary>
    /// Finds the index of the first item that matches a specified predicate.
    /// </summary>
    /// <param name="match">The predicate to use for searching.</param>
    /// <returns>The index of the first item that matches the predicate, or -1 if no match is found.</returns>
    public int FindIndex(Predicate<T> match) => _list.FindIndex(match);

    /// <summary>
    /// Returns a new collection containing the distinct items from the current collection.
    /// </summary>
    /// <returns>A new collection containing the distinct items.</returns>
    public DomainCollection<T> Distinct() => new(_list.Distinct());

    /// <summary>
    /// Shuffles the items in the collection.
    /// </summary>
    public void Shuffle()
    {
        var rng = new Random();
        var n = _list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            (_list[n], _list[k]) = (_list[k], _list[n]);
        }
    }

    /// <summary>
    /// Converts the collection to an array.
    /// </summary>
    /// <returns>An array containing the items from the collection.</returns>
    public T[] ToArray() => _list.ToArray();

    /// <summary>
    /// Returns a new collection containing the union of the current collection with another.
    /// </summary>
    /// <param name="other">The other collection to union with.</param>
    /// <returns>A new collection containing the union of the two collections.</returns>
    public DomainCollection<T> Union(DomainCollection<T> other) => new(_list.Union(other._list));

    /// <summary>
    /// Returns a new collection containing the intersection of the current collection with another.
    /// </summary>
    /// <param name="other">The other collection to intersect with.</param>
    /// <returns>A new collection containing the intersection of the two collections.</returns>
    public DomainCollection<T> Intersect(DomainCollection<T> other) => new(_list.Intersect(other._list));

    /// <summary>
    /// Returns a new collection containing the difference between the current collection and another.
    /// </summary>
    /// <param name="other">The other collection to find the difference with.</param>
    /// <returns>A new collection containing the difference between the two collections.</returns>
    public DomainCollection<T> Except(DomainCollection<T> other) => new(_list.Except(other._list));

    /// <summary>
    /// Returns a new collection containing the symmetric difference between the current collection and another.
    /// </summary>
    /// <param name="other">The other collection to find the symmetric difference with.</param>
    /// <returns>A new collection containing the symmetric difference between the two collections.</returns>
    public DomainCollection<T> SymmetricDifference(DomainCollection<T> other) => Union(other).Except(Intersect(other));

    /// <summary>
    /// Combines two collections using the union operation.
    /// </summary>
    /// <param name="a">The first collection.</param>
    /// <param name="b">The second collection.</param>
    /// <returns>A new collection containing the union of the two collections.</returns>
    public static DomainCollection<T> operator +(DomainCollection<T> a, DomainCollection<T> b) => a.Union(b);

    /// <summary>
    /// Returns a new collection containing the difference between two collections.
    /// </summary>
    /// <param name="a">The first collection.</param>
    /// <param name="b">The second collection.</param>
    /// <returns>A new collection containing the difference between the two collections.</returns>
    public static DomainCollection<T> operator -(DomainCollection<T> a, DomainCollection<T> b) => a.Except(b);

    /// <summary>
    /// Returns an enumerator for the collection.
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    /// <summary>
    /// Returns an enumerator for the collection (explicit non-generic version).
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
}