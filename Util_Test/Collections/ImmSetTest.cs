using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;

[TestFixture]
public class ImmSetTest
{
    [Test]
    public void Set_ArgumentArray_Basic()
    {
        ImmOrderedSet<byte> set = Imm.SetOf(_3_, _5_, _7_);
        Verify357(set);
    }

    [Test]
    public void Set_ArgumentArray_FromArray()
    {
        byte[]        bytes = [_3_, _5_, _7_];
        ImmOrderedSet<byte> set  = Imm.SetOf(bytes);
        Verify357(set);
    }

    [Test]
    public void Set_ArgumentArray_FromArrayModified()
    {
        byte[]        bytes = [_3_, _5_, _7_];
        ImmOrderedSet<byte> set  = Imm.SetOf(bytes);
        Verify357(set);

        bytes[0] = 77;
        bytes[1] = 88;
        bytes[2] = 99;

        Verify357(set);
    }

    [Test]
    public void SortedSet_ArgumentArray_Basic()
    {
        ImmOrderedSet<byte> set = Imm.SortedSetOf(_7_, _3_, _5_);
        Verify357(set);
    }

    [Test]
    public void SortedSet_ArgumentArray_FromArray()
    {
        byte[]        bytes = [_7_, _3_, _5_];
        ImmOrderedSet<byte> set  = Imm.SortedSetOf(bytes);
        Verify357(set);
    }

    [Test]
    public void SortedSet_ArgumentArray_FromArrayModified()
    {
        byte[]        bytes = [_7_, _3_, _5_];
        ImmOrderedSet<byte> set  = Imm.SortedSetOf(bytes);
        Verify357(set);

        bytes[0] = 77;
        bytes[1] = 88;
        bytes[2] = 99;

        Verify357(set);
    }

    private static void Verify357(ImmOrderedSet<byte> set)
    {
        set.Verify
        (
            s => s[0].ShouldBe(_3_),
            s => s[1].ShouldBe(_5_),
            s => s[2].ShouldBe(_7_)
        );
    }



    private static IEnumerable<ulong> MakeEnumerable5()
    {
        yield return 77uL;
        yield return 99uL;
        yield return 55uL;
        yield return 88uL;
        yield return 66uL;
    }


    [Test]
    public void Set_Easy_fromTrueEnumerable()
    {
        IEnumerable<ulong> source = MakeEnumerable5();
        var                set    = source.ToImmSet();
        Verify_55_99(set);
    }

    [Test]
    public void Set_Easy_fromEnumerableList()
    {
        IEnumerable<ulong> list = new List<ulong> { 88uL, 99uL, 66uL, 55uL, 77uL };
        var                set  = list.ToImmSet();
        Verify_55_99(set);
    }

    [Test]
    public void Set_Easy_fromOrderedEnumerable()
    {
        IOrderedEnumerable<ulong> oe  = MakeEnumerable5().Order();
        ImmOrderedSet<ulong>      set = oe.ToImmSet();
        Verify_55_99(set);
    }

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
            x => x.Contains(55uL).ShouldBeTrue(),
            x => x.Contains(66uL).ShouldBeTrue(),
            x => x.Contains(77uL).ShouldBeTrue(),
            x => x.Contains(88uL).ShouldBeTrue(),
            x => x.Contains(99uL).ShouldBeTrue(),
            x => x.Contains(100uL).ShouldBeFalse()
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
            x => x.IndexOf(55uL).ShouldBe(0),
            x => x.IndexOf(66uL).ShouldBe(1),
            x => x.IndexOf(77uL).ShouldBe(2),
            x => x.IndexOf(88uL).ShouldBe(3),
            x => x.IndexOf(99uL).ShouldBe(4),
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
