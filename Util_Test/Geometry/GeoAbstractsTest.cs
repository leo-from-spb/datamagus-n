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
        Assert.That(p.shift(s), Is.EqualTo(new GeoPoint(27.cm(), 15.cm())));
    }


    [Test]
    public void Corners()
    {
        GeoRect rect = new GeoRect(_1_cm, _1_mm, _3_cm, _3_mm);

        Assert.That(rect.Center, Is.EqualTo(point(_2_cm, _2_mm)));

        Assert.That(rect.LT, Is.EqualTo(point(_1_cm, _1_mm)));
        Assert.That(rect.CT, Is.EqualTo(point(_2_cm, _1_mm)));
        Assert.That(rect.RT, Is.EqualTo(point(_3_cm, _1_mm)));
        Assert.That(rect.LC, Is.EqualTo(point(_1_cm, _2_mm)));
        Assert.That(rect.RC, Is.EqualTo(point(_3_cm, _2_mm)));
        Assert.That(rect.LB, Is.EqualTo(point(_1_cm, _3_mm)));
        Assert.That(rect.CB, Is.EqualTo(point(_2_cm, _3_mm)));
        Assert.That(rect.RB, Is.EqualTo(point(_3_cm, _3_mm)));
    }

}
