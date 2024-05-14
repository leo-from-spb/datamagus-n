using System;
using System.Collections.Generic;

namespace Util.Collections;


/// <summary>
/// Base collection interface with covariant type.
/// </summary>
/// <typeparam name="T">element type (covariant).</typeparam>
public interface LCollection<out T> : IReadOnlyCollection<T>
{
    /// <summary>
    /// Count of elements.
    /// </summary>
    public new int Count { get; }

    /// <summary>
    /// Whether this collection has at least one element.
    /// </summary>
    public bool IsNotEmpty { get; }

    /// <summary>
    /// Whether this collection is empty.
    /// </summary>
    public bool IsEmpty { get; }

    /// <summary>
    /// Checks whether this collection contains at least one element,
    /// that meets the specified predicate.
    /// </summary>
    /// <param name="predicate">the predicate to check elements.</param>
    /// <returns>true, when found.</returns>
    /// <seealso cref="RCollection{T}.Contains(T)"/>
    public bool Contains(Predicate<T> predicate);


    int IReadOnlyCollection<T>.Count => Count;

}


/// <summary>
/// List of elements.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
public interface LList<out T> : LCollection<T>, IReadOnlyList<T>
{
    /// <summary>
    /// Item at the given index.
    /// </summary>
    /// <param name="index">index, zero-based.</param>
    public T At(int index);

    /// <summary>
    /// Item at the given index.
    /// </summary>
    /// <param name="index">index, zero-based.</param>
    T IReadOnlyList<T>.this [int index] => At(index);

    /// <summary>
    /// The first element.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">when this collection is empty.</exception>
    public T First { get; }

    /// <summary>
    /// The last element.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">when this collection is empty.</exception>
    public T Last { get; }

    /// <summary>
    /// Finds the index of the first occurance of an element that meets the specified predicate.
    /// </summary>
    /// <param name="predicate">the predicate to check elements.</param>
    /// <param name="notFound">the pseudo-index, returned when the element is not found.</param>
    /// <returns>the index of the element, or the pseudo-index when the element is not found.</returns>
    /// <seealso cref="RList{T}.IndexOf(T,int)"/>
    public int IndexOf(Predicate<T> predicate, int notFound = int.MinValue);

    /// <summary>
    /// Finds the index of the last occurance of an element that meets the specified predicate.
    /// </summary>
    /// <param name="predicate">the predicate to check elements.</param>
    /// <param name="notFound">the pseudo-index, returned when the element is not found.</param>
    /// <returns>the index of the element, or the pseudo-index when the element is not found.</returns>
    /// <seealso cref="RList{T}.LastIndexOf(T,int)"/>
    public int LastIndexOf(Predicate<T> predicate, int notFound = int.MinValue);

}


/// <summary>
/// Lite set is a set of elements — all elements are different.
/// LSet has two differences from RSet:
/// <ul>
/// <li>LSet is co-variant when RSet is not, and</li>
/// <li>LSet has no functions for quick element search (and for quick element presence check).</li>
/// </ul>
/// Both differences are imposed by the limitations of the C# language.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
/// <seealso cref="RSet{T}"/>
public interface LSet<out T> : LCollection<T>;


/// <summary>
/// A lite set of elements where the elements stay in a stable predicted order.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
/// <seealso cref="LSet{T}"/>
/// <seealso cref="LSortedSet{T}"/>
/// <seealso cref="ROrderSet{T}"/>
public interface LOrderSet<out T> : LSet<T>, LList<T>;


/// <summary>
/// A lite set of elements where the elements are sorted using the native (type-default) order.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
/// <seealso cref="LOrderSet{T}"/>
/// <seealso cref="RSortedSet{T}"/>
public interface LSortedSet<out T> : LOrderSet<T>
    where T : IComparable<T>;


