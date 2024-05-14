using System;

namespace Util.Collections;

/// <summary>
/// Base collection interface with invariant type.
/// </summary>
/// <typeparam name="T">element type (invariant).</typeparam>
public interface RCollection<T> : LCollection<T>
{

    /// <summary>
    /// Checks whether this collection contains the given element.
    /// The complexity of this operation depends on the implementation;
    /// in non-set implementations ic could be <i>O(n)</i>.
    /// </summary>
    /// <param name="element">the element to find.</param>
    /// <returns>true, when the given element presents in this collection.</returns>
    public bool Contains(T element);

}


/// <summary>
/// List of elements.
/// </summary>
/// <typeparam name="T">element type (invariant).</typeparam>
public interface RList<T> : RCollection<T>, LList<T>
{

    /// <summary>
    /// Finds the index of the first occurance of the given element.
    /// </summary>
    /// <param name="element">the element to look for.</param>
    /// <param name="notFound">the pseudo-index, returned when the element is not found.</param>
    /// <returns>the index of the element, or the pseudo-index when the element is not found.</returns>
    public int IndexOf(T element, int notFound = int.MinValue);

    /// <summary>
    /// Finds the index of the last occurance of the given element.
    /// </summary>
    /// <param name="element">the element to look for.</param>
    /// <param name="notFound">the pseudo-index, returned when the element is not found.</param>
    /// <returns>the index of the element, or the pseudo-index when the element is not found.</returns>
    public int LastIndexOf(T element, int notFound = int.MinValue);

}



/// <summary>
/// Set of elements.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
public interface RSet<T> : RCollection<T>, LSet<T>; //, IReadOnlySet<T>


/// <summary>
/// A set where the elements stay in a stable predicted order.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
public interface ROrderSet<T> : RSet<T>, RList<T>, LOrderSet<T>;


/// <summary>
/// A set of elements, that are sorted using the native (type-default) order.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
public interface RSortedSet<T> : ROrderSet<T>, LSortedSet<T>
    where T : IComparable<T>
{



}

