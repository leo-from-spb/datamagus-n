using static Util.Geometry.Dimensions;

namespace Util.Geometry;

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

    public GeoRect(int x1, int y1, int x2, int y2, Unit unit)
    {
        X1 = new Dim(x1, unit);
        Y1 = new Dim(y1, unit);
        X2 = new Dim(x2, unit);
        Y2 = new Dim(y2, unit);
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

    /// <summary>
    /// Checks whether this rectangle is degenerated, in other words, it's collapsed into a line segment.
    /// </summary>
    public bool IsDegenerated => X1.L == X2.L || Y1.L == Y2.L;

    /// <summary>
    /// Checks whether this rectangle intersects with <paramref name="that"/> rectangle.
    /// </summary>
    /// <param name="that">the rectangle to check intersection with.</param>
    public bool Overlaps(GeoRect that) => (X1 <= that.X2 && that.X1 <= X2)
                                       && (Y1 <= that.Y2 && that.Y1 <= Y2);

    /// <summary>
    /// Calculates the overlapping rectangle of this rectangle with another given rectangle.
    /// </summary>
    /// <param name="that">another rectangle to calculate overlapping with.</param>
    /// <returns>the result overlapping, or null when the rectangles don't overlap.</returns>
    public GeoRect? OverlapRect(GeoRect that)
    {
        Dim x1 = max(this.X1, that.X1);
        Dim y1 = max(this.Y1, that.Y1);
        Dim x2 = min(this.X2, that.X2);
        Dim y2 = min(this.Y2, that.Y2);
        return x1 <= x2 && y1 <= y2 ? new GeoRect(x1, y1, x2, y2) : null;
    }

}
