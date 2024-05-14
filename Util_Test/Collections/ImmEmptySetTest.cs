namespace Util.Collections;


[TestFixture]
public class ImmEmptySetTest
{

    [Test]
    public void EmptyListSet_Basic()
    {
        ROrderSet<ulong> empty = ImmEmptySet<ulong>.Instance;

        empty.Verify
        (
            e => e.IsEmpty.ShouldBeTrue(),
            e => e.IsNotEmpty.ShouldBeFalse(),
            e => e.Count.ShouldBe(0),
            e => e.Contains(0uL).ShouldBeFalse(),
            e => e.Contains(_ => true).ShouldBeFalse(),
            e => e.IndexOf(26uL).ShouldBeNegative(),
            e => e.IndexOf(_ => true).ShouldBeNegative(),
            e => e.LastIndexOf(26uL).ShouldBeNegative(),
            e => e.LastIndexOf(_ => true).ShouldBeNegative()
        );
    }

    [Test]
    public void EmptyListSet_WhenNotFound()
    {
        ROrderSet<ulong> empty = ImmEmptySet<ulong>.Instance;

        empty.Verify
        (
            e => e.IndexOf(42uL, -101).ShouldBe(-101),
            e => e.IndexOf(_ => true, -102).ShouldBe(-102),
            e => e.LastIndexOf(42uL, -103).ShouldBe(-103),
            e => e.LastIndexOf(_ => true, -104).ShouldBe(-104)
        );
    }

    [Test]
    public void EmptySortedSet_Basic()
    {
        RSortedSet<ulong> empty = ImmEmptySortedSet<ulong>.Instance;

        empty.Verify
        (
            e => e.IsEmpty.ShouldBeTrue(),
            e => e.IsNotEmpty.ShouldBeFalse(),
            e => e.Count.ShouldBe(0),
            e => e.Contains(0uL).ShouldBeFalse(),
            e => e.Contains(_ => true).ShouldBeFalse(),
            e => e.IndexOf(26uL).ShouldBeNegative(),
            e => e.IndexOf(_ => true).ShouldBeNegative(),
            e => e.LastIndexOf(26uL).ShouldBeNegative(),
            e => e.LastIndexOf(_ => true).ShouldBeNegative()
        );
    }

    [Test]
    public void EmptySortedSet_WhenNotFound()
    {
        RSortedSet<ulong> empty = ImmEmptySortedSet<ulong>.Instance;

        empty.Verify
        (
            e => e.IndexOf(42uL, -101).ShouldBe(-101),
            e => e.IndexOf(_ => true, -102).ShouldBe(-102),
            e => e.LastIndexOf(42uL, -103).ShouldBe(-103),
            e => e.LastIndexOf(_ => true, -104).ShouldBe(-104)
        );
    }

}