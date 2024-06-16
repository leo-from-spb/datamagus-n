using System.Collections.Generic;

namespace Util.Collections;


[TestFixture]
public class ConstSingletonSetTest
{

    [Test]
    public void SingletonSet_Basic()
    {
        var sing = new ConstSingletonSortedSet<ulong>(26uL);

        sing.Verify
        (
            s => s.Element.ShouldBe(26uL),
            s => s.IsNotEmpty.ShouldBeTrue(),
            s => s.IsEmpty.ShouldBeFalse(),
            s => s.Count.ShouldBe(1),
            s => s.Contains(26uL).ShouldBeTrue(),
            s => s.Contains(2uL).ShouldBeFalse(),
            s => s.Contains(x => x == 26uL).ShouldBeTrue(),
            s => s.Contains(x => x == 3uL).ShouldBeFalse(),
            s => s.IndexOf(26uL).ShouldBe(0),
            s => s.IndexOf(1uL).ShouldBeNegative(),
            s => s.IndexOf(x => x == 26uL).ShouldBe(0),
            s => s.IndexOf(x => x == 1uL).ShouldBeNegative(),
            s => s.LastIndexOf(26uL).ShouldBe(0),
            s => s.LastIndexOf(1uL).ShouldBeNegative(),
            s => s.LastIndexOf(x => x == 26uL).ShouldBe(0),
            s => s.LastIndexOf(x => x == 1uL).ShouldBeNegative()
        );
    }

    [Test]
    public void SingletonSet_Basic_R()
    {
        ROrderSet<ulong> sing = new ConstSingletonSortedSet<ulong>(26uL);

        sing.Verify
        (
            s => s.IsNotEmpty.ShouldBeTrue(),
            s => s.IsEmpty.ShouldBeFalse(),
            s => s.Count.ShouldBe(1),
            s => s.Contains(26uL).ShouldBeTrue(),
            s => s.Contains(2uL).ShouldBeFalse(),
            s => s.Contains(x => x == 26uL).ShouldBeTrue(),
            s => s.Contains(x => x == 3uL).ShouldBeFalse(),
            s => s.IndexOf(26uL).ShouldBe(0),
            s => s.IndexOf(1uL).ShouldBeNegative(),
            s => s.IndexOf(x => x == 26uL).ShouldBe(0),
            s => s.IndexOf(x => x == 1uL).ShouldBeNegative(),
            s => s.LastIndexOf(26uL).ShouldBe(0),
            s => s.LastIndexOf(1uL).ShouldBeNegative(),
            s => s.LastIndexOf(x => x == 26uL).ShouldBe(0),
            s => s.LastIndexOf(x => x == 1uL).ShouldBeNegative()
        );
    }


    [Test]
    public void SingletonSet_WhenNotFound()
    {
        ROrderSet<ulong> sing = new ConstSingletonSet<ulong>(42uL);

        sing.Verify
        (
            s => s.IndexOf(13uL, -101).ShouldBe(-101),
            s => s.LastIndexOf(13uL, -103).ShouldBe(-103)
        );
    }


    [Test]
    public void SingletonSet_Enumerate()
    {
        IEnumerable<ulong> sing = new ConstSingletonSortedSet<ulong>(1000uL);

        List<ulong> result = new List<ulong>();

        foreach (ulong e in sing)
            result.Add(e);

        result.Verify(
            r => r.ShouldContain(1000uL),
            r => r.Count.ShouldBe(1)
        );
    }

}
