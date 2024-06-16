namespace Util.Collections;


[TestFixture]
public class ConstEmptySetTest
{

    [Test]
    public void EmptyListSet_Basic_L()
    {
        LOrderSet<ulong> empty = ConstEmptySet<ulong>.Instance;

        empty.Verify
        (
            e => e.IsEmpty.ShouldBeTrue(),
            e => e.IsNotEmpty.ShouldBeFalse(),
            e => e.Count.ShouldBe(0),
            e => e.Contains(_ => true).ShouldBeFalse(),
            e => e.IndexOf(_ => true).ShouldBeNegative(),
            e => e.LastIndexOf(_ => true).ShouldBeNegative()
        );
    }

    [Test]
    public void EmptyListSet_Basic_R()
    {
        ROrderSet<ulong> empty = ConstEmptySet<ulong>.Instance;

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
    public void EmptyListSet_Basic_Imm()
    {
        ImmOrderSet<ulong> empty = ConstEmptySet<ulong>.Instance;

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
        ROrderSet<ulong> empty = ConstEmptySet<ulong>.Instance;

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
        RSortedSet<ulong> empty = ConstEmptySortedSet<ulong>.Instance;

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
        RSortedSet<ulong> empty = ConstEmptySortedSet<ulong>.Instance;

        empty.Verify
        (
            e => e.IndexOf(42uL, -101).ShouldBe(-101),
            e => e.IndexOf(_ => true, -102).ShouldBe(-102),
            e => e.LastIndexOf(42uL, -103).ShouldBe(-103),
            e => e.LastIndexOf(_ => true, -104).ShouldBe(-104)
        );
    }

}
