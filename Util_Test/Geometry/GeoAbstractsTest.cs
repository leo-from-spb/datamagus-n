using Util.Geometry;
using static Util.Geometry.Dimensions;

namespace Util.Test.Geometry;

[TestFixture]
public class GeoAbstractsTest
{

    [Test]
    public void Point_Shift()
    {
        GeoPoint p = new GeoPoint(20.cm, 10.cm);
        GeoSize  s = new GeoSize(7.cm, 5.cm);
        p.shift(s).ShouldBe(new GeoPoint(27.cm, 15.cm));
    }


    [Test]
    public void Size_Degenerated()
    {
        new GeoSize(1.cm, 2.cm).IsDegenerated.ShouldBeFalse();
        new GeoSize(0.mk, 2.cm).IsDegenerated.ShouldBeTrue();
        new GeoSize(3.cm, 0.mk).IsDegenerated.ShouldBeTrue();
    }

}
