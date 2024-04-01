using Util.Common.Geometry;
using static NUnit.Framework.Assert;
using static Util.Common.Geometry.Dimensions;
using static Util.Common.Geometry.Unit;

namespace Util.Test.Geometry;

[TestFixture]
public class GeoRectTest
{
    //    1   2  3   4  5   6   7  8             11        14   16
    //    ┌──────────┐  ┌──────────┐   1         ┌──────────┐
    //    │          │  │          │             │     P    │
    //    │          │  │          │   2         │   ┌───┐  ├───┐
    //    │    A     │  │     B    │             │   │ Q │  │ R │
    //    │      ┌───┴──┴───┐      │   3         │   └───┘  ├───┘
    //    │      │          │      │             │          │
    //    └──────┤          ├──────┘   4         │   ┌───┐  │
    //           │     X    │                    │   │ Y │  │
    //    ┌──────┤          ├──────┐   5         └───┤   ├──┘
    //    │      │          │      │                 │   │
    //    │      └───┬──┬───┘      │   6             └───┘
    //    │    C     │  │     D    │
    //    │          │  │          │   7
    //    │          │  │          │
    //    └──────────┘  └──────────┘   8

    private GeoRect Rect_A = new GeoRect(_1_cm, _1_cm, _4_cm, _4_cm);
    private GeoRect Rect_B = new GeoRect(_5_cm, _1_cm, _8_cm, _4_cm);
    private GeoRect Rect_C = new GeoRect(_1_cm, _5_cm, _4_cm, _8_cm);
    private GeoRect Rect_D = new GeoRect(_5_cm, _5_cm, _8_cm, _8_cm);
    private GeoRect Rect_X = new GeoRect(_3_cm, _3_cm, _6_cm, _6_cm);
    private GeoRect Rect_P = new GeoRect(11, 1, 14, 5, cm);
    private GeoRect Rect_Q = new GeoRect(12, 2, 13, 3, cm);
    private GeoRect Rect_R = new GeoRect(14, 2, 16, 3, cm);
    private GeoRect Rect_Y = new GeoRect(12, 4, 13, 6, cm);


    [Test]
    public void Overlaps_Basic()
    {
        Multiple(() =>
        {
            That(Rect_A.Overlaps(Rect_B), Is.False);
            That(Rect_B.Overlaps(Rect_A), Is.False);
            That(Rect_A.Overlaps(Rect_C), Is.False);
            That(Rect_C.Overlaps(Rect_A), Is.False);
            That(Rect_B.Overlaps(Rect_D), Is.False);
            That(Rect_D.Overlaps(Rect_B), Is.False);
            That(Rect_C.Overlaps(Rect_D), Is.False);
            That(Rect_D.Overlaps(Rect_C), Is.False);

            That(Rect_A.Overlaps(Rect_X), Is.True);
            That(Rect_B.Overlaps(Rect_X), Is.True);
            That(Rect_C.Overlaps(Rect_X), Is.True);
            That(Rect_D.Overlaps(Rect_X), Is.True);

            That(Rect_X.Overlaps(Rect_A), Is.True);
            That(Rect_X.Overlaps(Rect_B), Is.True);
            That(Rect_X.Overlaps(Rect_C), Is.True);
            That(Rect_X.Overlaps(Rect_D), Is.True);

            That(Rect_P.Overlaps(Rect_P), Is.True);

            That(Rect_P.Overlaps(Rect_Q), Is.True);
            That(Rect_Q.Overlaps(Rect_P), Is.True);

            That(Rect_P.Overlaps(Rect_Y), Is.True);
            That(Rect_Y.Overlaps(Rect_P), Is.True);
        });
    }

    [Test]
    public void Overlaps_Border()
    {
        Multiple(() =>
        {
            That(Rect_P.Overlaps(Rect_R), Is.True);
            That(Rect_R.Overlaps(Rect_P), Is.True);
        });
    }

}
