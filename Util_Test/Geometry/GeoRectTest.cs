using Util.Common.Geometry;
using static NUnit.Framework.Assert;
using static Util.Common.Geometry.Dimensions;
using static Util.Common.Geometry.GeoAbstracts;
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

    private GeoRect[] AllRects;


    public GeoRectTest()
    {
        AllRects = [Rect_A, Rect_B, Rect_C, Rect_D, Rect_X, Rect_P, Rect_Q, Rect_R, Rect_Y];
    }


    [Test]
    public void Create_FromPointAndSize()
    {
        var p = new GeoPoint(_1_mm, _2_mm);
        var s = new GeoSize(_3_mm, _4_mm);
        var r = new GeoRect(p, s);

        r.ShouldSatisfyAllConditions(() =>
        {
            r.X1.ShouldBe(_1_mm);
            r.Y1.ShouldBe(_2_mm);
            r.X2.ShouldBe(_4_mm);
            r.Y2.ShouldBe(_6_mm);
        });
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

    [Test]
    public void Degenerated()
    {
        Rect_X.IsDegenerated.ShouldBeFalse();

        (new GeoRect(_1_cm, _2_cm, _1_cm, _3_cm)).IsDegenerated.ShouldBeTrue();
        (new GeoRect(_1_cm, _5_cm, _3_cm, _5_cm)).IsDegenerated.ShouldBeTrue();
    }

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
        true.ShouldSatisfyAllConditions
        (
            () => rect1.Overlaps(rect2).ShouldBe(overlap),
            () => rect2.Overlaps(rect1).ShouldBe(overlap)
        );

    [Test]
    public void OverlappingRect_Basic()
    {
        Multiple(() =>
        {
            CheckOverlappingRect(Rect_A, Rect_X, new GeoRect(3, 3, 4, 4, cm));
            CheckOverlappingRect(Rect_B, Rect_X, new GeoRect(5, 3, 6, 4, cm));
            CheckOverlappingRect(Rect_C, Rect_X, new GeoRect(3, 5, 4, 6, cm));
            CheckOverlappingRect(Rect_D, Rect_X, new GeoRect(5, 5, 6, 6, cm));
        });
    }

    [Test]
    public void OverlappingRect_Complex()
    {
        Multiple(() =>
        {
            CheckOverlappingRect(Rect_P, Rect_Q, Rect_Q);
            CheckOverlappingRect(Rect_P, Rect_R, new GeoRect(14, 2, 14, 3, cm));
            CheckOverlappingRect(Rect_P, Rect_Y, new GeoRect(12, 4, 13, 5, cm));
        });
    }

    [Test]
    public void OverlappingRect_DonT()
    {
        Multiple(() =>
        {
            CheckOverlappingRect(Rect_A, Rect_B, null);
            CheckOverlappingRect(Rect_B, Rect_C, null);
            CheckOverlappingRect(Rect_C, Rect_D, null);
        });
    }

    private static void CheckOverlappingRect(GeoRect rect1, GeoRect rect2, GeoRect? overlap)
    {
        Multiple(() =>
        {
            rect1.OverlapRect(rect2).ShouldBe(overlap);
            rect2.OverlapRect(rect1).ShouldBe(overlap);
        });
    }


    [Test]
    public void OverlappingConsistency() => AllRects.ShouldSatisfyAllConditions(() =>
    {
        foreach (var r1 in AllRects)
        {
            foreach (var r2 in AllRects)
            {
                bool     yes     = r1.Overlaps(r2);
                GeoRect? overlap = r1.OverlapRect(r2);
                if (yes) overlap.ShouldNotBeNull();
                else     overlap.ShouldBeNull();
            }
        }
    });
}
