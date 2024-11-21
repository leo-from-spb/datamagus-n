using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;

[TestFixture]
public class ImmSortSetTest
{
    [Test]
    public void Easy_fromArray()
    {
        ulong[] array = [55, 66, 77, 88, 99];
        var     set   = array.ToImmSortSet();
        Verify_55_99(set);
    }

    [Test]
    public void Easy_fromSortedSet()
    {
        var ss  = new SortedSet<ulong>();
        ss.Add(55L);
        ss.Add(66L);
        ss.Add(77L);
        ss.Add(88L);
        ss.Add(99L);
        var set = ss.ToImmSortSet();
        Verify_55_99(set);
    }

    [Test]
    public void Regular_fromArray()
    {
        ulong[] array = { 77L, 88L, 99L, 55L, 66L, 77L, 66L, 88L, 55L };
        var     set   = array.ToImmSortSet();
        Verify_55_99(set);
    }

    private void Verify_55_99(ImmSortSet<ulong> set) =>
        set.Verify
        (
            x => x.IsNotEmpty.ShouldBeTrue(),
            x => x.IsEmpty.ShouldBeFalse(),
            x => x.Count.ShouldBe(5),
            x => x.IndexOf(55L).ShouldBe(0),
            x => x.IndexOf(66L).ShouldBe(1),
            x => x.IndexOf(77L).ShouldBe(2),
            x => x.IndexOf(88L).ShouldBe(3),
            x => x.IndexOf(99L).ShouldBe(4),
            x => x.ToArray().ShouldBeEquivalentTo(new ulong[] {55, 66, 77, 88, 99})
        );



    [Test]
    public void Empty_fromArray()
    {
        var x = (new ulong[] { }).ToImmSortSet();
        VerifyEmptySortSet(x);
    }

    [Test]
    public void Empty_fromSet()
    {
        var set = new SortedSet<ulong>();
        var x   = set.ToImmSortSet();
        VerifyEmptySortSet(x);
    }

    private void VerifyEmptySortSet(ImmSortSet<ulong> set) =>
        set.Verify
        (
            x => x.IsEmpty.ShouldBeTrue(),
            x => x.IsNotEmpty.ShouldBeFalse(),
            x => x.Count.ShouldBe(0),
            x => x.IndexOf(1380L).ShouldBeNegative(),
            x => x.Contains(99L).ShouldBeFalse()
        );


}
