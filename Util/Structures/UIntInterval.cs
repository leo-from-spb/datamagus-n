using System;

namespace Util.Structures;


/// <summary>
/// Interval.
/// </summary>
public readonly struct UIntInterval : IEquatable<UIntInterval>
{
    /// <summary>
    /// Minimal value.
    /// </summary>
    public readonly uint Min;

    /// <summary>
    /// Maximal value.
    /// </summary>
    public readonly uint Max;

    /// <summary>
    /// Trivial constructor.
    /// </summary>
    /// <param name="min">the minimal (first) value.</param>
    /// <param name="max">the maximal (last) value, inclusive.</param>
    public UIntInterval(uint min, uint max)
    {
        Min = min;
        Max = max;
    }

    /// <summary>
    /// Lengths of the interval (including both <see cref="Min"/> and <see cref="Max"/>).
    /// </summary>
    public uint Length => Max - Min + 1;

    /// <summary>
    /// Checks whether this interval contains the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(uint value) => value >= Min && value <= Max;


    public bool Equals(UIntInterval that)
    {
        return Min == that.Min && Max == that.Max;
    }

    public override bool Equals(object? obj)
    {
        return obj is UIntInterval other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Min, Max);
    }

    public override string ToString()
    {
        return $"$Min..$Max";
    }

    public void Deconstruct(out uint min, out uint max)
    {
        min = Min;
        max = Max;
    }
}
