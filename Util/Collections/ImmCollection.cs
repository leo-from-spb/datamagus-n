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

    protected static readonly EqualityComparer<T> Eqr = EqualityComparer<T>.Default;

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



/// <summary>
/// List and Set of a single element.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ImmSingletonSet<T> : ImmCollection<T>, ROrderSet<T>
{
    /// <summary>
    /// The single element.
    /// </summary>
    public readonly T Element;

    public ImmSingletonSet(T element)
    {
        this.Element = element;
    }


    public override bool IsNotEmpty => true;
    public override bool IsEmpty    => false;
    public override int  Count      => 1;

    public T First => Element;
    public T Last  => Element;

    public T At(int index) =>
        index == 0
            ? Element
            : throw new IndexOutOfRangeException($"Attempted top get {index}-th element from a singleton set");

    public int IndexOf(T element, int notFound = Int32.MinValue) =>
        Eqr.Equals(Element, element) ? 0 : notFound;

    public int LastIndexOf(T element, int notFound = Int32.MinValue) =>
        Eqr.Equals(Element, element) ? 0 : notFound;

    public int IndexOf(Predicate<T> predicate, int notFound = Int32.MinValue) =>
        predicate(Element) ? 0 : notFound;

    public int LastIndexOf(Predicate<T> predicate, int notFound = Int32.MinValue) =>
        predicate(Element) ? 0 : notFound;

    public override bool Contains(T element) => Eqr.Equals(Element, element);

    public override bool Contains(Predicate<T> predicate) => predicate(Element);

    public override IEnumerator<T> GetEnumerator() => new SingletonEnumerator<T>(this);



    private class SingletonEnumerator<E> : IEnumerator<E>
    {
        private readonly ImmSingletonSet<E> collection;

        private sbyte position = -1;

        internal SingletonEnumerator(ImmSingletonSet<E> collection)
        {
            this.collection = collection;
        }

        public bool MoveNext()
        {
            if (position < +1) position++;
            return position == 0;
        }

        public E Current =>
            position switch
            {
                0  => collection.Element,
                -1 => throw new InvalidOperationException("The current enumerator position is before the element"),
                +1 => throw new InvalidOperationException("The current enumerator position is after the element"),
                _  => throw new Exception($"The current position is unexpected: {position}")
            };

        #nullable disable
        object IEnumerator.Current => Current;
        #nullable restore

        public void Reset()
        {
            position = -1;
        }

        public void Dispose() { }
    }

}


/// <summary>
/// Sorted Set of a single element.
/// </summary>
/// <typeparam name="E">element type.</typeparam>
public class ImmSingletonSortedSet<E> : ImmSingletonSet<E>, RSortedSet<E>
    where E : IComparable<E>
{
    public ImmSingletonSortedSet(E element) : base(element) { }
}
