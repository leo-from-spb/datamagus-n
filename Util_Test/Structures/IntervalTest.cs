using static Util.Structures.Intervals;

namespace Util.Structures;


[TestFixture]
public class IntervalTest
{
    [Test]
    public void Basic()
    {
        var interval = new Interval<ulong>(200uL, 500uL);

        interval.Verify
        (
            i => i.Min.ShouldBe(200uL),
            i => i.Max.ShouldBe(500uL),
            i => i.IsProper.ShouldBeTrue(),
            i => i.IsSingle.ShouldBeFalse(),
            i => i.Contains(100uL).ShouldBeFalse(),
            i => i.Contains(200uL).ShouldBeTrue(),
            i => i.Contains(300uL).ShouldBeTrue(),
            i => i.Contains(500uL).ShouldBeTrue(),
            i => i.Contains(600uL).ShouldBeFalse(),
            i => i.ToString().ShouldBe("200..500")
        );
    }

    [Test]
    public void BasicString()
    {
        var interval = new Interval<string>("Dasha", "Masha");

        interval.Verify
        (
            i => i.IsProper.ShouldBeTrue(),
            i => i.IsSingle.ShouldBeFalse(),
            i => i.Contains("Bob").ShouldBeFalse(),
            i => i.Contains("Dasha").ShouldBeTrue(),
            i => i.Contains("Glasha").ShouldBeTrue(),
            i => i.Contains("Masha").ShouldBeTrue(),
            i => i.Contains("Sasha").ShouldBeFalse(),
            i => i.ToString().ShouldBe("Dasha..Masha")
        );
    }


    [Test]
    public void Comparison()
    {
        var intervalA = IntervalOf(100, 200);
        var intervalB = IntervalOf(100, 300);
        var intervalC = IntervalOf(100, 300);
        var intervalD = IntervalOf(150, 250);

        Verify
        (
            () => (intervalA <= intervalB).ShouldBeTrue(),
            () => (intervalB <= intervalC).ShouldBeTrue(),
            () => (intervalB < intervalC).ShouldBeFalse(),
            () => (intervalB <= intervalD).ShouldBeTrue(),
            () => (intervalB >= intervalA).ShouldBeTrue(),
            () => (intervalC >= intervalB).ShouldBeTrue(),
            () => (intervalC > intervalB).ShouldBeFalse(),
            () => (intervalD >= intervalB).ShouldBeTrue()
        );
    }


    [Test]
    public void DeconstructString()
    {
        var interval = new Interval<string>("Dasha", "Masha");
        var (d, m) = interval;

        Verify
        (
            () => d.ShouldBe("Dasha"),
            () => m.ShouldBe("Masha")
        );
    }


    [Test]
    public void Equals()
    {
        var interval1 = new Interval<ulong>(200uL, 500uL);
        var interval2 = new Interval<ulong>(200uL, 500uL);

        (interval1 == interval2).ShouldBeTrue();
        (interval1 != interval2).ShouldBeFalse();
    }


    [Test]
    public void IsIn()
    {
        var interval = new Interval<int>(200, 500);
        Verify
        (
            () => 100.IsIn(interval).ShouldBeFalse(),
            () => 300.IsIn(interval).ShouldBeTrue()
        );
    }


    [Test]
    public void Length()
    {
        IntervalOf(10,20).Length().ShouldBe(11);
        IntervalOf(10uL,20uL).Length().ShouldBe(11uL);
        IntervalOf(-1L,+1L).Length().ShouldBe(3L);
    }

    [Test]
    public void LengthOfSingle()
    {
        IntervalOf(10,10).Length().ShouldBe(1);
        IntervalOf(long.MinValue, long.MinValue).Length().ShouldBe(1L);
        IntervalOf(ulong.MaxValue, ulong.MaxValue).Length().ShouldBe(1uL);
    }

    [Test]
    public void LengthOfInvertedSigned()
    {
        IntervalOf(+10,-10).Length().ShouldBe(0);
        IntervalOf(1000uL,3uL).Length().ShouldBe(0uL);
    }


    [Test]
    public void Intersect()
    {
        var intervalA = IntervalOf(10, 60);
        var intervalB = IntervalOf(30, 70);
        var ovl = intervalA.Intersection(intervalB);
        ovl.HasValue.ShouldBeTrue();
        ovl.ShouldBe(IntervalOf(30, 60));
    }

    [Test]
    public void IntersectSingle()
    {
        var intervalA = IntervalOf(10, 40);
        var intervalB = IntervalOf(40, 70);
        var ovl = intervalA.Intersection(intervalB);
        ovl.HasValue.ShouldBeTrue();
        ovl.ShouldBe(IntervalOf(40, 40));
    }

    [Test]
    public void IntersectNo()
    {
        var intervalA = IntervalOf(10, 40);
        var intervalB = IntervalOf(50, 70);
        var ovl = intervalA.Intersection(intervalB);
        ovl.ShouldBeNull();
    }
}
