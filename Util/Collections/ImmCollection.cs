using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Util.Collections.Implementation;
using static Util.Collections.ImmConst;


namespace Util.Collections;


/// <summary>
/// Immutable collection.
/// Any implementation of this class must guarantee that the content is immutable.
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
[CollectionBuilder(typeof(Imm), nameof(Imm.CreateImmList))]
public interface ImmCollection<T> : IReadOnlyCollection<T>
{
    /// <summary>
    /// Inner implementation.
    /// </summary>
    internal ImmutableCollection<T> Imp { get; }

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
}



/// <summary>
/// Immutable sequence — a collection that preserves the order of elements.
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
[CollectionBuilder(typeof(Imm), nameof(Imm.CreateImmList))]
public interface ImmSeq<T> : ImmCollection<T>
{
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
}



/// <summary>
/// Immutable list — a collection that preserves the order of elements and also allows the direct access by index.
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
[CollectionBuilder(typeof(Imm), nameof(Imm.CreateImmList))]
public interface ImmList<T> : ImmSeq<T>, IReadOnlyList<T>
{
    /// <summary>
    /// Item at the given index.
    /// </summary>
    /// <param name="index">index, zero-based.</param>
    /// <exception cref="IndexOutOfRangeException">when the index is wrong or the collection is empty.</exception>
    public T At(int index);

    /// <summary>
    /// Finds the first occurrence of the given element in the list.
    /// If the element is found, returns its index (zero-based),
    /// otherwise returns a negative number.
    /// </summary>
    /// <param name="element">the element to find.</param>
    /// <returns>index of the element, or a negative number when not found.</returns>
    public int IndexOf(T element) => IndexOf(element, notFoundIndex);

    /// <summary>
    /// Finds the first occurrence of the given element in the list.
    /// If the element is found, returns its index (zero-based),
    /// otherwise returns the value of the parameter <paramref name="notFound"/>.
    /// </summary>
    /// <param name="element">the element to find.</param>
    /// <param name="notFound">what to return when the given element is not found.</param>
    /// <returns>index of the element, or <paramref name="notFound"/> when not found.</returns>
    public int IndexOf(T element, int notFound);

    /// <summary>
    /// Finds the last occurrence of the given element in the list.
    /// If the element is found, returns its index (zero-based),
    /// otherwise returns a negative number.
    /// </summary>
    /// <param name="element">the element to find.</param>
    /// <returns>index of the element, or a negative number when not found.</returns>
    public int LastIndexOf(T element) => LastIndexOf(element, notFoundIndex);

    /// <summary>
    /// Finds the last occurrence of the given element in the list.
    /// If the element is found, returns its index (zero-based),
    /// otherwise returns the value of the parameter <paramref name="notFound"/>.
    /// </summary>
    /// <param name="element">the element to find.</param>
    /// <param name="notFound">what to return when the given element is not found.</param>
    /// <returns>index of the element, or <paramref name="notFound"/> when not found.</returns>
    public int LastIndexOf(T element, int notFound);

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
/// It also provides fast function <b>Contains(T)</b> that is declared in <see cref="IReadOnlySet{T}"/>.
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
[CollectionBuilder(typeof(Imm), nameof(Imm.CreateImmListSet))]
public interface ImmSet<T> : ImmCollection<T>, IReadOnlySet<T>
{
    /// <summary>
    /// Union set of two sets.
    /// The result is a view to the given sets.
    /// </summary>
    /// <param name="setA">the first set.</param>
    /// <param name="setB">the second set.</param>
    /// <returns>the resulted union set.</returns>
    public static ImmSet<T> operator +(ImmSet<T> setA, ImmSet<T> setB)
        => CollectionLogic.UnionSet(setA, setB);

    /// <summary>
    /// Union set of a set and an array.
    /// The result is a view to the given sets.
    /// </summary>
    /// <param name="setA">the set.</param>
    /// <param name="arrayB">the array.</param>
    /// <returns>the resulted union set.</returns>
    public static ImmSet<T> operator +(ImmSet<T> setA, T[] arrayB)
        => CollectionLogic.UnionSet(setA, arrayB);
}



/// <summary>
/// Immutable ordered set of elements, in which elements have a meaningful order.
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
public interface ImmOrdSet<T> : ImmSet<T>, ImmSeq<T>
{ }



/// <summary>
/// Immutable ordered set of elements, that also has direct access by index.
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
[CollectionBuilder(typeof(Imm), nameof(Imm.CreateImmListSet))]
public interface ImmListSet<T> : ImmOrdSet<T>, ImmList<T>
{ }



/// <summary>
/// Immutable set where all elements are sorted by default order.
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
[CollectionBuilder(typeof(Imm), nameof(Imm.CreateImmSortedListSet))]
public interface ImmSortedSet<T> : ImmOrdSet<T>
    where T : IComparable<T>
{ }



/// <summary>
/// Immutable set with direct access to its elements and where all elements are sorted by default order.
/// </summary>
/// <typeparam name="T">type of elements.</typeparam>
[CollectionBuilder(typeof(Imm), nameof(Imm.CreateImmSortedListSet))]
public interface ImmSortedListSet<T> : ImmSortedSet<T>, ImmListSet<T>
    where T : IComparable<T>
{ }
