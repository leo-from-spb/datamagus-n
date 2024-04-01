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



/// <summary>
/// Rectangle.
/// </summary>
public readonly struct GeoRect
{
    public readonly Dim X1;
    public readonly Dim Y1;
    public readonly Dim X2;
    public readonly Dim Y2;


    public GeoRect(Dim x1, Dim y1, Dim x2, Dim y2)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }

    public GeoRect(GeoPoint corner, GeoSize size) : this(corner.X, corner.Y, corner.X + size.W, corner.Y + size.H) { }

    public GeoPoint Center => new GeoPoint((X1 + X2) / 2, (Y1 + Y2) / 2);
    public GeoPoint LT     => new GeoPoint(X1, Y1);
    public GeoPoint CT     => new GeoPoint((X1 + X2) / 2, Y1);
    public GeoPoint RT     => new GeoPoint(X2, Y1);
    public GeoPoint RC     => new GeoPoint(X2, (Y1 + Y2)/2);
    public GeoPoint RB     => new GeoPoint(X2, Y2);
    public GeoPoint CB     => new GeoPoint((X1 + X2) / 2, Y2);
    public GeoPoint LB     => new GeoPoint(X1, Y2);
    public GeoPoint LC     => new GeoPoint(X1, (Y1 + Y2)/2);

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