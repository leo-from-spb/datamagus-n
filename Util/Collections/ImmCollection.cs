using System;
using System.Collections.Generic;

namespace Util.Collections;


/// <summary>
/// Immutable collection.
/// Any implementation of this class must guarantee that the content is immutable.
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
public interface ImmCollection<T> : IReadOnlyCollection<T>
{
    /// <summary>
    /// Inner implementation.
    /// </summary>
    protected ImmutableCollection<T> Imp { get; }

    /// <summary>
    /// This collection has at least one element.
    /// </summary>
    public bool IsNotEmpty { get; }

    /// <summary>
    /// This collection is empty.
    /// </summary>
    public bool IsEmpty { get; }

    /// <summary>
    /// Finds an element, that matched the given <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">predicate to check elements.</param>
    /// <returns>the result.</returns>
    public Found<T> Find(Predicate<T> predicate);

    /// <summary>
    /// Check whether the collection contains at least one element that matches the given <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">the predicate to match.</param>
    /// <returns>whether an element exists in the collection.</returns>
    public bool Contains(Predicate<T> predicate);

    /// <summary>
    /// Check whether the collection the given <paramref name="element"/>.
    /// The performance of the method depends on the implementation of the collection.
    /// </summary>
    /// <param name="element">the element to check.</param>
    /// <returns>whether the element exists in the collection.</returns>
    public bool Contains(T element);
}


/// <summary>
/// Immutable list.
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
public interface ImmList<T> : ImmCollection<T>, IReadOnlyList<T>
{
    /// <summary>
    /// Item at the given index.
    /// </summary>
    /// <param name="index">index, zero-based.</param>
    /// <exception cref="IndexOutOfRangeException">when the index is wrong or the collection is empty.</exception>
    public T At(int index);

    /// <summary>
    /// The first element.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">when this collection is empty</exception>
    /// <seealso cref="Last"/>
    public T First { get; }

    /// <summary>
    /// The last element.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">when this collection is empty</exception>
    /// <seealso cref="First"/>
    public T Last { get; }

    /// <summary>
    /// Finds the first occurrence of the given element in the list.
    /// If the element is found, returns its index (zero-based),
    /// otherwise returns the value of the parameter <paramref name="notFound"/>.
    /// </summary>
    /// <param name="element">the element to find.</param>
    /// <param name="notFound">what to return when the given element is not found.</param>
    /// <returns>index of the element, or <paramref name="notFound"/> when not found.</returns>
    public int IndexOf(T element, int notFound = int.MinValue);

    /// <summary>
    /// Finds the last occurrence of the given element in the list.
    /// If the element is found, returns its index (zero-based),
    /// otherwise returns the value of the parameter <paramref name="notFound"/>.
    /// </summary>
    /// <param name="element">the element to find.</param>
    /// <param name="notFound">what to return when the given element is not found.</param>
    /// <returns>index of the element, or <paramref name="notFound"/> when not found.</returns>
    public int LastIndexOf(T element, int notFound = int.MinValue);

    /// <summary>
    /// Finds an element, that matched the given <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">predicate to check elements.</param>
    /// <param name="fromIndex">index to start the search from.</param>
    /// <returns>the result.</returns>
    public Found<T> FindFirst(Predicate<T> predicate, int fromIndex = 0);

    /// <summary>
    /// Finds an element, that matched the given <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">predicate to check elements.</param>
    /// <param name="fromIndex">index to start the search from.</param>
    /// <returns>the result.</returns>
    public Found<T> FindLast(Predicate<T> predicate);
}



/// <summary>
/// Immutable set of elements, all elements are different.
/// It also provides fast function <see cref="ImmCollection{T}.Contains(T)"/>
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
public interface ImmSet<T> : ImmCollection<T>
{ }


/// <summary>
/// Immutable ordered set of elements, in which elements have a meaningful order.
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
public interface ImmOrderedSet<T> : ImmSet<T>, ImmList<T>
{ }



public interface ImmSortedSet<T> : ImmOrderedSet<T>
    where T : IComparable<T>
{ }
