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
            CheckRectsOverlapped(Rect_A, Rect_B, false);
            CheckRectsOverlapped(Rect_A, Rect_C, false);
            CheckRectsOverlapped(Rect_A, Rect_D, false);
            CheckRectsOverlapped(Rect_B, Rect_C, false);
            CheckRectsOverlapped(Rect_B, Rect_D, false);
            CheckRectsOverlapped(Rect_C, Rect_D, false);
            
            CheckRectsOverlapped(Rect_A, Rect_X, true);
            CheckRectsOverlapped(Rect_B, Rect_X, true);
            CheckRectsOverlapped(Rect_C, Rect_X, true);
            CheckRectsOverlapped(Rect_D, Rect_X, true);
            
            CheckRectsOverlapped(Rect_P, Rect_Q, true);
            CheckRectsOverlapped(Rect_P, Rect_Y, true);
            
            CheckRectsOverlapped(Rect_Q, Rect_Y, false);
        });
    }

    [Test]
    public void Overlaps_Border()
    {
        CheckRectsOverlapped(Rect_R, Rect_P, true);
    }

    private static void CheckRectsOverlapped(GeoRect rect1, GeoRect rect2, bool overlap) =>
        Multiple(() =>
        {
            That(rect1.Overlaps(rect2), Is.EqualTo(overlap));
            That(rect2.Overlaps(rect1), Is.EqualTo(overlap));
        });
}
