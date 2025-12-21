using System;
using System.Diagnostics;
using System.Globalization;
using static System.Math;

namespace Util.Geometry;

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
    extension(int length) {
        public Dim mk => new Dim(length, Unit.mk);
        public Dim mm => new Dim(length, Unit.mm);
        public Dim cm => new Dim(length, Unit.cm);
        public Dim dm => new Dim(length, Unit.dm);
    }

    extension(double length) {
        public Dim mk => new Dim(length, Unit.mk);
        public Dim mm => new Dim(length, Unit.mm);
        public Dim cm => new Dim(length, Unit.cm);
        public Dim dm => new Dim(length, Unit.dm);
    }


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
