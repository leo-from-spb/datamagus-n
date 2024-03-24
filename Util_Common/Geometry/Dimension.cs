using System.Diagnostics;
using static System.Math;

namespace Util.Common.Geometry;

public readonly struct Dimension : IEquatable<Dimension>, IComparable<Dimension>
{
    /// <summary>
    /// Geometric dimension.
    /// Stores the value in microns.
    /// </summary>
    public readonly int L;


    private Dimension(int lengthInMicrons)
    {
        this.L = lengthInMicrons;
    }

    private Dimension(long lengthInMicrons)
    {
        Debug.Assert(lengthInMicrons is > int.MinValue and <= int.MaxValue);
        this.L = (int)lengthInMicrons;
    }

    private Dimension(double lengthInMicrons)
    {
        Debug.Assert(lengthInMicrons is > int.MinValue and <= int.MaxValue);
        this.L = (int)lengthInMicrons;
    }

    public Dimension(int length, Unit unit)
    {
        byte unitIndex = (byte)unit;
        Debug.Assert(unitIndex < UnitFactors.Length);
        long longDistance = length * UnitFactors[unitIndex];
        Debug.Assert(longDistance is > int.MinValue and <= int.MaxValue);
        this.L = (int)longDistance;
    }

    public Dimension(double length, Unit unit)
    {
        byte unitIndex = (byte)unit;
        Debug.Assert(unitIndex < UnitFactors.Length);
        double doubleDistance = double.Round(length * UnitFactors[unitIndex]);
        Debug.Assert(doubleDistance is > int.MinValue and <= int.MaxValue);
        this.L = (int)doubleDistance;
    }

    private static readonly uint[] UnitFactors = new uint[] { 1, 1000, 10000, 100000 };


    public int  CompareTo(Dimension that) => this.L.CompareTo(that.L);
    public bool Equals(Dimension that) => this.L == that.L;

    public static bool operator == (Dimension a, Dimension b) => a.L == b.L;
    public static bool operator != (Dimension a, Dimension b) => a.L != b.L;

    public static bool operator <  (Dimension a, Dimension b) => a.L < b.L;
    public static bool operator <= (Dimension a, Dimension b) => a.L <= b.L;
    public static bool operator >= (Dimension a, Dimension b) => a.L >= b.L;
    public static bool operator >  (Dimension a, Dimension b) => a.L > b.L;

    public static Dimension operator + (Dimension a, Dimension b) => new Dimension(a.L + b.L);
    public static Dimension operator - (Dimension a, Dimension b) => new Dimension(a.L - b.L);

    public static Dimension operator * (Dimension d, int f) => new Dimension(d.L * f);
    public static Dimension operator / (Dimension d, int f) => new Dimension(d.L / f);
    public static Dimension operator * (Dimension d, float f) => new Dimension(d.L * f);
    public static Dimension operator / (Dimension d, float f) => new Dimension(d.L / f);
    public static Dimension operator * (Dimension d, double f) => new Dimension(d.L * f);
    public static Dimension operator / (Dimension d, double f) => new Dimension(d.L / f);

    public override int GetHashCode() => Abs(this.L);
    
}



public static class Dimensions
{
    public static Dimension mk(this int length) => new Dimension(length, Unit.mk);
    public static Dimension mm(this int length) => new Dimension(length, Unit.mm);
    public static Dimension cm(this int length) => new Dimension(length, Unit.cm);
    public static Dimension dm(this int length) => new Dimension(length, Unit.dm);

    public static Dimension mk(this double length) => new Dimension(length, Unit.mk);
    public static Dimension mm(this double length) => new Dimension(length, Unit.mm);
    public static Dimension cm(this double length) => new Dimension(length, Unit.cm);
    public static Dimension dm(this double length) => new Dimension(length, Unit.dm);


    public static readonly Dimension _1_mm = 1.mm();
    public static readonly Dimension _2_mm = 2.mm();
    public static readonly Dimension _3_mm = 3.mm();
    public static readonly Dimension _4_mm = 4.mm();
    public static readonly Dimension _5_mm = 5.mm();
    public static readonly Dimension _6_mm = 6.mm();
    public static readonly Dimension _7_mm = 7.mm();
    public static readonly Dimension _8_mm = 8.mm();
    public static readonly Dimension _9_mm = 9.mm();

    public static readonly Dimension _1_cm = 1.cm();
    public static readonly Dimension _2_cm = 2.cm();
    public static readonly Dimension _3_cm = 3.cm();
    public static readonly Dimension _4_cm = 4.cm();
    public static readonly Dimension _5_cm = 5.cm();
    public static readonly Dimension _6_cm = 6.cm();
    public static readonly Dimension _7_cm = 7.cm();
    public static readonly Dimension _8_cm = 8.cm();
    public static readonly Dimension _9_cm = 9.cm();

}


/// <summary>
/// Distance units.
/// </summary>
public enum Unit : byte
{
    mk,
    mm,
    cm,
    dm
}
