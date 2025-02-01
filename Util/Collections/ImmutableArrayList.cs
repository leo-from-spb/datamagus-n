using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;


/// <summary>
/// Immutable array-based list.
/// Non-empty.
/// </summary>
internal class ImmutableArrayList<T> : Immutable, ImmList<T>
{
    protected static readonly EqualityComparer<T> eq = EqualityComparer<T>.Default;

    /// <summary>
    /// Elements. Non-empty.
    /// </summary>
    protected readonly T[] Elements;

    /// <summary>
    ///Number of elements.
    /// </summary>
    public readonly int Count;

    /// <summary>
    /// Internal constructor — takes the given arrays as is and doesn't copy it.
    /// It takes the ownership of this array, so nothing must change the arrays after the call of this constructor.
    /// </summary>
    /// <param name="elements">array with elements.</param>
    internal ImmutableArrayList(T[] elements)
    {
        this.Elements = elements;
        Count = elements.Length;
    }

    public Immutable Imp => this;

    public bool IsNotEmpty => true;
    public bool IsEmpty    => false;

    int IReadOnlyCollection<T>.Count => Count;

    public T this[int index] => Elements[index];

    public T First => Elements[0];
    public T Last  => Elements[Count-1];

    public T At(int index) => Elements[index];

    public virtual bool Contains(T element) => Elements.Contains(element);

    public bool Contains(Predicate<T> predicate)
    {
        for (int i = 0; i < Count; i++)
            if (predicate(this[i])) return true;
        return false;
    }

    public virtual int IndexOf(T element, int notFound = int.MinValue)
    {
        for (int i = 0; i < Count; i++)
            if (eq.Equals(Elements[i], element)) return i;
        return notFound;
    }

    public virtual int LastIndexOf(T element, int notFound = int.MinValue)
    {
        for (int i = Count - 1; i >= 0; i--)
            if (eq.Equals(Elements[i], element)) return i;
        return notFound;
    }

    public Found<T> Find(Predicate<T> predicate) => FindFirst(predicate, 0);

    public Found<T> FindFirst(Predicate<T> predicate, int fromIndex = 0)
    {
        for (int i = fromIndex; i < Count; i++)
        {
            T element = Elements[i];
            if (predicate(element)) return new Found<T>(true, element);
        }

        return Found<T>.NotFound;
    }

    public Found<T> FindLast(Predicate<T> predicate)
    {
        for (int i = Count-1; i >= 0; i--)
        {
            T element = Elements[i];
            if (predicate(element)) return new Found<T>(true, element);
        }
        
        return Found<T>.NotFound;
    }

    public IEnumerator<T>   GetEnumerator() => Elements.AsEnumerable().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Elements.GetEnumerator();
}


/// <summary>
/// Array-based immutable set.
/// All elements are expected to be different.
/// </summary>
internal class ImmutableArraySet<T> : ImmutableArrayList<T>, ImmOrderedSet<T>
{
    /// <summary>
    /// Internal constructor.
    /// The caller must guarantee that all elements are different.
    /// </summary>
    /// <param name="elements">array of different elements.</param>
    internal ImmutableArraySet(T[] elements) : base(elements) { }
}


/// <summary>
/// Array-based sorted immutable set.
/// </summary>
internal class ImmutableSortedSet<T> : ImmutableArraySet<T>, ImmSortedSet<T>
    where T : IComparable<T>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="elements">sorted array.</param>
    internal ImmutableSortedSet(T[] elements) : base(elements) { }

    public override bool Contains(T element)
    {
        int index = Array.BinarySearch(Elements, element);
        return index >= 0;
    }

    public override int IndexOf(T element, int notFound = int.MinValue)
    {
        int index = Array.BinarySearch(Elements, element);
        return index >= 0 ? index : notFound;
    }

    public override int LastIndexOf(T element, int notFound = int.MinValue) => IndexOf(element, notFound);
}
