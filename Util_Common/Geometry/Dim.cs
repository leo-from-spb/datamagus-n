using System.Diagnostics;
using System.Globalization;
using static System.Math;

namespace Util.Common.Geometry;

/// <summary>
/// Geometric dimension.
/// Stores the value in microns.
/// </summary>
public readonly struct Dim : IEquatable<Dim>, IComparable<Dim>
{
    /// <summary>
    /// The value in microns.
    /// </summary>
    public readonly int L;


    private Dim(int lengthInMicrons)
    {
        this.L = lengthInMicrons;
    }

    private Dim(long lengthInMicrons)
    {
        if (lengthInMicrons is > int.MinValue and <= int.MaxValue) this.L = (int)lengthInMicrons;
        else throw new StackOverflowException($"The value is too big or too small: {lengthInMicrons} µ");
    }

    private Dim(double lengthInMicrons)
    {
        if (lengthInMicrons is > int.MinValue and <= int.MaxValue) this.L = (int)lengthInMicrons;
        else throw new StackOverflowException($"The value is too big or too small: {lengthInMicrons} µ");
    }

    /// <summary>
    /// Specified dimension in given units.
    /// </summary>
    /// <param name="length">the value.</param>
    /// <param name="unit">the unit.</param>
    /// <exception cref="StackOverflowException"> when the value is too large or too small.</exception>
    public Dim(int length, Unit unit)
    {
        byte unitIndex = (byte)unit;
        Debug.Assert(unitIndex < UnitFactors.Length);
        long theDim = length * UnitFactors[unitIndex];
        if (theDim is > int.MinValue and <= int.MaxValue) this.L = (int)theDim;
        else throw new StackOverflowException($"The value too big or too small: {length} {UnitMarkers[unitIndex]}");
    }

    /// <summary>
    /// Specified dimension in given units.
    /// </summary>
    /// <param name="length">the value.</param>
    /// <param name="unit">the unit.</param>
    /// <exception cref="StackOverflowException"> when the value is too large or too small.</exception>
    public Dim(double length, Unit unit)
    {
        byte unitIndex = (byte)unit;
        Debug.Assert(unitIndex < UnitFactors.Length);
        double theDim = double.Round(length * UnitFactors[unitIndex]);
        if (theDim is > int.MinValue and <= int.MaxValue) this.L = (int)theDim;
        else throw new StackOverflowException($"The value too big or too small: {length} {UnitMarkers[unitIndex]}");
    }

    
    private static readonly uint[]   UnitFactors = [1,   1000, 10000, 100000];
    private static readonly string[] UnitMarkers = ["µ", "mm", "cm",  "m"   ];


    public          int  CompareTo(Dim  that) => this.L.CompareTo(that.L);
    public          bool Equals(Dim     that) => this.L == that.L;
    public override bool Equals(object? obj)  => obj is Dim that && Equals(that);

    public static bool operator == (Dim a, Dim b) => a.L == b.L;
    public static bool operator != (Dim a, Dim b) => a.L != b.L;

    public static bool operator <  (Dim a, Dim b) => a.L < b.L;
    public static bool operator <= (Dim a, Dim b) => a.L <= b.L;
    public static bool operator >= (Dim a, Dim b) => a.L >= b.L;
    public static bool operator >  (Dim a, Dim b) => a.L > b.L;

    public static Dim operator + (Dim a, Dim b) => new Dim(a.L + b.L);
    public static Dim operator - (Dim a, Dim b) => new Dim(a.L - b.L);

    public static Dim operator * (Dim d, int f) => new Dim(d.L * f);
    public static Dim operator / (Dim d, int f) => new Dim(d.L / f);
    public static Dim operator * (Dim d, float f) => new Dim(d.L * f);
    public static Dim operator / (Dim d, float f) => new Dim(d.L / f);
    public static Dim operator * (Dim d, double f) => new Dim(d.L * f);
    public static Dim operator / (Dim d, double f) => new Dim(d.L / f);

    public override int GetHashCode() => Abs(this.L);

    public override string ToString() => L.ToString("# ### ### ##0", NumberFormat).TrimStart();

    // STATIC

    private static readonly NumberFormatInfo NumberFormat;

    static Dim()
    {
        NumberFormat = new NumberFormatInfo()
                       {
                           NumberDecimalSeparator = ".",
                           NumberGroupSeparator   = " "
                       };
    }
}



public static class Dimensions
{
    public static Dim mk(this int length) => new Dim(length, Unit.mk);
    public static Dim mm(this int length) => new Dim(length, Unit.mm);
    public static Dim cm(this int length) => new Dim(length, Unit.cm);
    public static Dim dm(this int length) => new Dim(length, Unit.dm);

    public static Dim mk(this double length) => new Dim(length, Unit.mk);
    public static Dim mm(this double length) => new Dim(length, Unit.mm);
    public static Dim cm(this double length) => new Dim(length, Unit.cm);
    public static Dim dm(this double length) => new Dim(length, Unit.dm);


    public static readonly Dim _0_mk = 0.mk();
    public static readonly Dim _1_mk = 1.mk();

    public static readonly Dim _1_mm = 1.mm();
    public static readonly Dim _2_mm = 2.mm();
    public static readonly Dim _3_mm = 3.mm();
    public static readonly Dim _4_mm = 4.mm();
    public static readonly Dim _5_mm = 5.mm();
    public static readonly Dim _6_mm = 6.mm();
    public static readonly Dim _7_mm = 7.mm();
    public static readonly Dim _8_mm = 8.mm();
    public static readonly Dim _9_mm = 9.mm();

    public static readonly Dim _1_cm = 1.cm();
    public static readonly Dim _2_cm = 2.cm();
    public static readonly Dim _3_cm = 3.cm();
    public static readonly Dim _4_cm = 4.cm();
    public static readonly Dim _5_cm = 5.cm();
    public static readonly Dim _6_cm = 6.cm();
    public static readonly Dim _7_cm = 7.cm();
    public static readonly Dim _8_cm = 8.cm();
    public static readonly Dim _9_cm = 9.cm();


    /// <summary>
    /// The smaller dimension of the two given.
    /// </summary>
    public static Dim min(Dim a, Dim b) => a.L <= b.L ? a : b;

    /// <summary>
    /// The larger dimension of the two given.
    /// </summary>
    public static Dim max(Dim a, Dim b) => a.L >= b.L ? a : b;


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
