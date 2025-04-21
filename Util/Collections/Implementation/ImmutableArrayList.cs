using System;
using System.Collections.Generic;
using System.Linq;
using static Util.Collections.ImmConst;

namespace Util.Collections.Implementation;


/// <summary>
/// Immutable array-based list.
/// Non-empty.
/// </summary>
internal class ImmutableArrayList<T> : ImmutableCollection<T>, ImmList<T>
{
    protected override string CollectionWord => "ArrayList";

    /// <summary>
    /// Elements. Non-empty.
    /// </summary>
    protected readonly T[] Elements;

    /// <summary>
    ///Number of elements.
    /// </summary>
    public override int Count { get; }

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


    /// <summary>
    /// Internal function that shares the inner array.
    /// Never use this function.
    /// </summary>
    internal T[] ShareElementsArray() => Elements;


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

    public int IndexOf(T element) => IndexOf(element, notFoundIndex);

    public virtual int IndexOf(T element, int notFound)
    {
        for (int i = 0; i < Count; i++)
            if (eq.Equals(Elements[i], element)) return i;
        return notFound;
    }

    public virtual int LastIndexOf(T element) => LastIndexOf(element, notFoundIndex);

    public virtual int LastIndexOf(T element, int notFound)
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

    public override IEnumerator<T> GetEnumerator() => Elements.AsEnumerable().GetEnumerator();


    public ImmListSet<T> ToSet()
    {
        if (this is ImmListSet<T> alreadyImmSet) return alreadyImmSet;

        var distinct = Elements.Deduplicate();
        int m        = distinct.Count;

        if (m == Count)
        {
            // our array has no duplicates
            return Count <= 3
                ? new ImmutableMiniSet<T>(Elements)
                : new ImmutableHashSet<T>(Elements);
        }
        else
        {
            // our array has duplicates, use the deduplicated list
            if (m == 1) return new ImmutableSingleton<T>(Elements[0]);

            T[] newArray = distinct.ToArray();
            return m <= 3
                ? new ImmutableMiniSet<T>(newArray)
                : new ImmutableHashSet<T>(newArray);
        }
    }
}

