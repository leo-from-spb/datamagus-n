using System.Diagnostics.CodeAnalysis;

namespace Util.Common.Geometry;

/// <summary>
/// Geometric Point.
/// </summary>
public readonly struct GeoPoint : IEquatable<GeoPoint>
{
    /// <summary>
    /// Coordinate X.
    /// </summary>
    public readonly Dim X;

    /// <summary>
    /// Coordinate Y.
    /// </summary>
    public readonly Dim Y;

    /// <summary>
    /// Initializes the instance.
    /// </summary>
    /// <param name="x">Coordinate X.</param>
    /// <param name="y">Coordinate Y.</param>
    public GeoPoint(Dim x, Dim y)
    {
        X = x;
        Y = y;
    }

    public override int GetHashCode() => X.L ^ Y.L;

    public bool Equals(GeoPoint other) => this.X == other.X && this.Y == other.Y;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is GeoPoint other && Equals(other);

    public static bool operator ==(GeoPoint a, GeoPoint b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(GeoPoint a, GeoPoint b) => a.X != b.X || a.Y != b.Y;

    public GeoPoint shift(GeoSize size) => new GeoPoint(X + size.W, Y + size.H);

}



/// <summary>
/// Geometric size of a figure (width and height).
/// </summary>
public readonly struct GeoSize
{
    /// <summary>
    /// Width.
    /// </summary>
    public readonly Dim W;

    /// <summary>
    /// Height.
    /// </summary>
    public readonly Dim H;



    public GeoSize(Dim w, Dim h)
    {
        W = w;
        H = h;
    }


    public override int GetHashCode() => W.L + H.L - (W.L ^ H.L);

    public bool Equals(GeoSize other) => this.W == other.W && this.H == other.H;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is GeoSize other && Equals(other);

    public static bool operator == (GeoSize a, GeoSize b) => a.W == b.W && a.H == b.H;
    public static bool operator != (GeoSize a, GeoSize b) => a.W != b.W || a.H != b.H;

}



public static class GeoAbstracts
{

    public static GeoPoint point(Dim x, Dim y) => new GeoPoint(x, y);

    public static GeoPoint point(int x, int y, Unit unit) => new GeoPoint(new Dim(x, unit), new Dim(y, unit));
    public static GeoPoint point(double x, double y, Unit unit) => new GeoPoint(new Dim(x, unit), new Dim(y, unit));

    public static GeoSize size(Dim w, Dim h) => new GeoSize(w, h);

    public static GeoSize size(int w, int h, Unit unit) => new GeoSize(new Dim(w, unit), new Dim(h, unit));
    public static GeoSize size(double w, double h, Unit unit) => new GeoSize(new Dim(w, unit), new Dim(h, unit));

}