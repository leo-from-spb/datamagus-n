using Util.Common.Geometry;
using static Util.Common.Geometry.Dimensions;
using static Util.Common.Geometry.GeoAbstracts;

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
    public void Corners()
    {
        GeoRect rect = new GeoRect(_1_cm, _1_mm, _3_cm, _3_mm);

        rect.ShouldSatisfyAllConditions
        (
            r => r.Center.ShouldBe(point(_2_cm, _2_mm)),
            r => r.LT.ShouldBe(point(_1_cm, _1_mm)),
            r => r.CT.ShouldBe(point(_2_cm, _1_mm)),
            r => r.RT.ShouldBe(point(_3_cm, _1_mm)),
            r => r.LC.ShouldBe(point(_1_cm, _2_mm)),
            r => r.RC.ShouldBe(point(_3_cm, _2_mm)),
            r => r.LB.ShouldBe(point(_1_cm, _3_mm)),
            r => r.CB.ShouldBe(point(_2_cm, _3_mm)),
            r => r.RB.ShouldBe(point(_3_cm, _3_mm))
        );
    }

}
