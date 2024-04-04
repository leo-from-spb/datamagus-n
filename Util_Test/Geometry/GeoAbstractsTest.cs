using Util.Common.Geometry;
using static Util.Common.Geometry.Dimensions;

namespace Util.Test.Geometry;

[TestFixture]
public class GeoAbstractsTest
{

    [Test]
    public void Point_Shift()
    {
        GeoPoint p = new GeoPoint(20.cm(), 10.cm());
        GeoSize  s = new GeoSize(7.cm(), 5.cm());
        p.shift(s).ShouldBe(new GeoPoint(27.cm(), 15.cm()));
    }


    [Test]
    public void Size_Degenerated()
    {
        new GeoSize(_1_cm, _2_cm).IsDegenerated.ShouldBeFalse();
        new GeoSize(_0_mk, _2_cm).IsDegenerated.ShouldBeTrue();
        new GeoSize(_3_cm, _0_mk).IsDegenerated.ShouldBeTrue();
    }

}
