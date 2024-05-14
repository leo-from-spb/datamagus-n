using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;


/// <summary>
/// Immutable collection base class.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
public abstract class ImmCollection<T> : RCollection<T>
{

    public abstract int  Count      { get; }
    public abstract bool IsNotEmpty { get; }
    public abstract bool IsEmpty    { get; }

    public abstract bool Contains(Predicate<T> predicate);
    public abstract bool Contains(T element);

    public abstract IEnumerator<T> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}



/// <summary>
/// Empty list and set.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
public class ImmEmptySet<T> : ImmCollection<T>, ROrderSet<T>
{
    /// <summary>
    /// The empty list and set.
    /// </summary>
    public static readonly ImmEmptySet<T> Instance = new ImmEmptySet<T>();

    public override bool IsNotEmpty => false;
    public override bool IsEmpty    => true;
    public override int  Count      => 0;

    public T First => throw new IndexOutOfRangeException("The set is empty when attempted to get the first element.");
    public T Last  => throw new IndexOutOfRangeException("The set is empty when attempted to get the last element.");

    public T At(int index) => throw new IndexOutOfRangeException($"The set is empty when attempted to get an element at index {index}.");

    public override bool Contains(Predicate<T> predicate) => false;

    public override bool Contains(T element) => false;

    public int IndexOf(Predicate<T> predicate, int notFound) => notFound;

    public int IndexOf(T element, int notFound) => notFound;

    public int LastIndexOf(Predicate<T> predicate, int notFound = int.MinValue) => notFound;

    public int LastIndexOf(T element, int notFound = int.MinValue) => notFound;

    public override IEnumerator<T> GetEnumerator() => Enumerable.Empty<T>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Array.Empty<T>().GetEnumerator();
}



/// <summary>
/// Empty sorted set.
/// </summary>
/// <typeparam name="T">element type.</typeparam>
public class ImmEmptySortedSet<T> : ImmEmptySet<T>, RSortedSet<T>
    where T : IComparable<T>
{
    /// <summary>
    /// The empty list and set.
    /// </summary>
    public new static readonly ImmEmptySortedSet<T> Instance = new ImmEmptySortedSet<T>();
}




