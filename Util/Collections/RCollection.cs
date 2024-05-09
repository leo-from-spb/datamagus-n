using System.Collections.Generic;

namespace Util.Collections;


/// <summary>
/// Base readable collection interface.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
public interface RCollection<out T> : IReadOnlyCollection<T>
{
    /// <summary>
    /// Count of items.
    /// </summary>
    public new int Count { get; }

    /// <summary>
    /// Whether this collection has at least one item.
    /// </summary>
    public bool IsNotEmpty { get; }

    /// <summary>
    /// Whether this collection is empty.
    /// </summary>
    public bool IsEmpty { get; }
}


/// <summary>
/// List of items.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
public interface RList<T> : RCollection<T>, IReadOnlyList<T>
{
    /// <summary>
    /// Item at the given index.
    /// </summary>
    /// <param name="index"></param>
    public T At(int index);

    /// <summary>
    /// Item at the given index.
    /// </summary>
    /// <param name="index"></param>
    public new T this [int index] => At(index);

    /// <summary>
    /// The first item.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">when this collection is empty.</exception>
    public T First { get; }

    /// <summary>
    /// The last item.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">when this collection is empty.</exception>
    public T Last { get; }

    /// <summary>
    /// Finds the index of the first occurance of the given item.
    /// </summary>
    /// <param name="item">the item to find.</param>
    /// <param name="notFound">the pseudo-index, returned when the item is not found.</param>
    /// <returns>the index of the item, or the pseudo-index when the item is not found.</returns>
    public int IndexOf(T item, int notFound = int.MinValue);
}


/// <summary>
/// Set of items.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
public interface RSet<T> : RCollection<T> //, IReadOnlySet<T>
{
    /// <summary>
    /// Checks whether the given item is in this set.
    /// </summary>
    /// <param name="item">the item to find.</param>
    /// <returns></returns>
    public /*new*/ bool Contains(T item);

}