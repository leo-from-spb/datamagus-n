using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;

[TestFixture]
public class ImmSetTest
{
    [Test]
    public void Set_Easy_fromArray()
    {
        ulong[] array = [55, 66, 77, 88, 99];
        var     set   = array.ToImmSet();
        Verify_55_99(set);
    }

    [Test]
    public void Set_Easy_fromCollection()
    {
        IReadOnlyCollection<ulong> collection = new List<ulong> { 77L, 55L, 99L, 88L, 66L };
        var set  = collection.ToImmSet();
        Verify_55_99(set);
    }

    [Test]
    public void Set_Easy_fromList()
    {
        var list = new List<ulong> { 77L, 55L, 99L, 88L, 66L };
        var set  = list.ToImmSet();
        Verify_55_99(set);
    }

    [Test]
    public void Set_Easy_fromHashSet()
    {
        var ss  = new HashSet<ulong> { 55L, 66L, 77L, 88L, 99L };
        var set = ss.ToImmSet();
        Verify_55_99(set);
    }

    [Test]
    public void Set_Easy_fromSortedSet()
    {
        var ss  = new SortedSet<ulong> { 55L, 66L, 77L, 88L, 99L };
        var set = ss.ToImmSet();
        Verify_55_99(set);
    }

    private void Verify_55_99(ImmSet<ulong> set) =>
        set.Verify
        (
            x => x.IsNotEmpty.ShouldBeTrue(),
            x => x.IsEmpty.ShouldBeFalse(),
            x => x.Count.ShouldBe(5),
            x => x.Contains(55L).ShouldBeTrue(),
            x => x.Contains(66L).ShouldBeTrue(),
            x => x.Contains(77L).ShouldBeTrue(),
            x => x.Contains(88L).ShouldBeTrue(),
            x => x.Contains(99L).ShouldBeTrue(),
            x => x.Contains(100L).ShouldBeFalse()
        );


    [Test]
    public void SortedSet_Easy_fromArray()
    {
        ulong[] array = [77L, 55L, 99L, 88L, 66L];
        var     set   = array.ToImmSortedSet();
        Verify_Sorted_55_99(set);
    }

    [Test]
    public void SortedSet_Easy_fromCollection()
    {
        IReadOnlyCollection<ulong> collection = new List<ulong> { 77L, 55L, 99L, 88L, 66L };

        var set = collection.ToImmSortedSet();
        Verify_Sorted_55_99(set);
    }

    [Test]
    public void SortedSet_Easy_fromSortedSet()
    {
        var ss  = new SortedSet<ulong> { 55L, 66L, 77L, 88L, 99L };
        var set = ss.ToImmSortedSet();
        Verify_Sorted_55_99(set);
    }

    [Test]
    public void SortedSet_Regular_fromArray()
    {
        ulong[] array = { 77L, 88L, 99L, 55L, 66L, 77L, 66L, 88L, 55L };
        var     set   = array.ToImmSortedSet();
        Verify_Sorted_55_99(set);
    }

    private void Verify_Sorted_55_99(ImmSortedSet<ulong> set) =>
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
        var x = (new ulong[] { }).ToImmSortedSet();
        VerifyEmptySortSet(x);
    }

    [Test]
    public void Empty_fromSet()
    {
        var set = new SortedSet<ulong>();
        var x   = set.ToImmSortedSet();
        VerifyEmptySortSet(x);
    }

    private void VerifyEmptySortSet(ImmSortedSet<ulong> set) =>
        set.Verify
        (
            x => x.IsEmpty.ShouldBeTrue(),
            x => x.IsNotEmpty.ShouldBeFalse(),
            x => x.Count.ShouldBe(0),
            x => x.IndexOf(1380L).ShouldBeNegative(),
            x => x.Contains(99L).ShouldBeFalse()
        );


}
