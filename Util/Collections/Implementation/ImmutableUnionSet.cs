using System;
using System.Collections.Generic;
using Util.Fun;

namespace Util.Collections.Implementation;


internal class ImmutableUnionSet<T> : ImmutableCollection<T>, ImmSet<T>
{
    protected override string CollectionWord => "UnionSet";

    /// <summary>
    /// The base set of elements.
    /// </summary>
    protected readonly ImmSet<T> A;

    /// <summary>
    /// Added elements.
    /// </summary>
    protected readonly ImmSet<T> B;

    /// <summary>
    /// Count of the elements.
    /// </summary>
    public override int Count { get; }

    /// <summary>
    /// True means the original sets don't overlap.
    /// </summary>
    private readonly bool strong;

    /// <summary>
    /// Instantiates the union set.
    /// </summary>
    public ImmutableUnionSet(ImmSet<T> setA, ImmSet<T> setB)
        : this(setA, setB, CountElements(setA, setB))
    { }

    internal ImmutableUnionSet(ImmSet<T> setA, ImmSet<T> setB, int count)
    {
        A = setA;
        B = setB;
        Count = count;
        strong = setA.Count + setB.Count == count;
    }

    private static int CountElements(ImmSet<T> setA, ImmSet<T> setB)
    {
        int nA = setA.Count,
            nB = setB.Count;
        if (nA < nB) return CountElements(setB, setA);
        int n = nA + nB;
        foreach (T el in setB)
            if (setA.Contains(el))
                n--;
        return n;
    }

    internal override byte CascadingLevel => Math.Max(A.Imp.CascadingLevel, B.Imp.CascadingLevel).Succ;

    public bool IsNotEmpty => Count != 0;
    public bool IsEmpty    => Count == 0;

    public bool Contains(T item)
        => A.Contains(item) || B.Contains(item);

    public bool Contains(Predicate<T> predicate)
        => A.Contains(predicate) || B.Contains(predicate);

    public Found<T> Find(Predicate<T> predicate)
    {
        var found = A.Find(predicate);
        if (!found.Ok) found = B.Find(predicate);
        return found;
    }

    public override IEnumerator<T> GetEnumerator()
    {
        foreach (T el in A)
        {
            yield return el;
        }
        foreach (T el in B)
        {
            if (strong || !A.Contains(el))
                yield return el;
        }
    }


    public bool Overlaps(IEnumerable<T> other)
    {
        foreach (T el in other)
            if (A.Contains(el) || B.Contains(el))
                return true;
        return false;
    }

    public bool SetEquals(IEnumerable<T> other)
    {
        int n = 0;
        foreach (T el in other)
        {
            n++;
            if (n > Count) return false;
            if (!A.Contains(el) && !B.Contains(el)) return false;
        }

        return n == Count;
    }


    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }
    public bool IsSubsetOf(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }
    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }
    public bool IsSupersetOf(IEnumerable<T> other)
    {
        throw new NotImplementedException();
    }
}
