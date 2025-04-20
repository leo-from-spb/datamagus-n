using System;
using System.Numerics;
using Util.Fun;

namespace Util.Structures;

/// <summary>
/// Generic interval, inclusive (both borders <see cref="Min"/> and <see cref="Max"/> are inclusive).
/// </summary>
public readonly struct Interval<X> : IComparable<Interval<X>>, IEquatable<Interval<X>>
    where X : IComparable<X>
{
    /// <summary>
    /// The minimal value.
    /// </summary>
    public readonly X Min;

    /// <summary>
    /// The maximal value.
    /// </summary>
    public readonly X Max;


    /// <summary>
    /// Trivial constructor.
    /// </summary>
    /// <param name="min">the minimal (first) value.</param>
    /// <param name="max">the maximal (last) value, inclusive.</param>
    public Interval(X min, X max)
    {
        Min = min;
        Max = max;
    }


    /// <summary>
    /// This interval is proper — it contains more than one values;
    /// in other words, it's not a single value, and it's not inverted.
    /// </summary>
    public bool IsProper => Min.CompareTo(Max) < 0;

    /// <summary>
    /// This interval contains only one value,
    /// in other words the <see cref="Min"/> is the same values as the <see cref="Max"/>.
    /// </summary>
    public bool IsSingle => Min.CompareTo(Max) == 0;


    /// <summary>
    /// Checks whether this interval contains the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value"></param>
    public bool Contains(X value)
        => value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;

    public bool Equals(Interval<X> that)
        => this.Min.Equals(that.Min) && this.Max.Equals(that.Max);

    public override bool Equals(object? obj)
        => obj is Interval<X> that && Equals(that);

    /// <summary>
    /// Compares first <see cref="Min"/> then <see cref="Max"/>.
    /// </summary>
    public int CompareTo(Interval<X> that)
    {
        int z = this.Min.CompareTo(that.Min);
        if (z == 0) z = this.Max.CompareTo(that.Max);
        return z;
    }

    public override int GetHashCode()
        => this.Min.GetHashCode() * 7 ^ this.Max.GetHashCode();

    public static bool operator == (Interval<X> a, Interval<X> b) => a.Equals(b);
    public static bool operator != (Interval<X> a, Interval<X> b) => !a.Equals(b);

    public static bool operator <= (Interval<X> a, Interval<X> b) => a.CompareTo(b) <= 0;
    public static bool operator >= (Interval<X> a, Interval<X> b) => a.CompareTo(b) >= 0;

    public static bool operator < (Interval<X> a, Interval<X> b) => a.CompareTo(b) < 0;
    public static bool operator > (Interval<X> a, Interval<X> b) => a.CompareTo(b) > 0;

    
    public void Deconstruct(out X min, out X max)
    {
        min = Min;
        max = Max;
    }

    public override string ToString() => $"{Min}..{Max}";
}



public static class Intervals
{
    /// <summary>
    /// Makes an interval with specified borders.
    /// </summary>
    /// <param name="min">the minimal (first) value.</param>
    /// <param name="max">the maximal (last) value, inclusive.</param>
    /// <returns>the created interval.</returns>
    public static Interval<X> IntervalOf<X>(X min, X max)
        where X : IComparable<X>
        => new Interval<X>(min, max);

    /// <summary>
    /// Checks whether this value is in the given interval.
    /// </summary>
    /// <param name="value">the value to check.</param>
    /// <param name="interval">the interval for checking.</param>
    public static bool IsIn<X>(this X value, Interval<X> interval)
        where X : IComparable<X>
        => interval.Contains(value);

    /// <summary>
    /// Length of the interval;
    /// in other words, the number of values in this interval.
    /// Applicable to integers only.
    /// </summary>
    /// <param name="interval">the interval to get length.</param>
    /// <typeparam name="X">an integer type.</typeparam>
    /// <returns>the length, or zero if this interval is invalid.</returns>
    public static X Length<X>(this Interval<X> interval)
        where X : IBinaryInteger<X>
    {
        var (min, max) = interval;
        int c = min.CompareTo(max);
        return c switch
               {
                   < 0 => max - min + X.One,
                   0   => X.One,
                   _   => X.Zero
               };
    }

    /// <summary>
    /// Finds the intersection of the two given intervals.
    /// </summary>
    /// <param name="ivA">the first interval.</param>
    /// <param name="ivB">the second interval.</param>
    /// <returns>the intersection, or null if the given intervals don't intersect.</returns>
    public static Interval<X>? Intersection<X>(this Interval<X> ivA, Interval<X> ivB)
        where X : IComparable<X>
    {
        var (minA, maxA) = ivA;
        var (minB, maxB) = ivB;
        var min = ComparableFun.Greatest(minA, minB);
        var max = ComparableFun.Least(maxA, maxB);
        if (min.CompareTo(max) <= 0) return new Interval<X>(min, max);
        else return null;
    }
}
