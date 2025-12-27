using System;
using System.Collections.Generic;
using System.Linq;
using Util.Collections.Implementation;

namespace Util.Collections;

[TestFixture]
public class ImmSetTest
{
    [Test]
    public void Set_ArgumentArray_Basic()
    {
        ImmListSet<byte> set = Imm.SetOf(_3_, _5_, _7_);
        Verify357(set);
    }

    [Test]
    public void Set_ArgumentArray_FromArray()
    {
        byte[]        bytes = [_3_, _5_, _7_];
        ImmListSet<byte> set  = Imm.SetOf(bytes);
        Verify357(set);
    }

    [Test]
    public void Set_ArgumentArray_FromArrayModified()
    {
        byte[]        bytes = [_3_, _5_, _7_];
        ImmListSet<byte> set  = Imm.SetOf(bytes);
        Verify357(set);

        bytes[0] = 77;
        bytes[1] = 88;
        bytes[2] = 99;

        Verify357(set);
    }

    [Test]
    public void Set_ByCollectionExpression_1()
    {
        ImmListSet<byte> set = [_3_, _5_, _7_];
        Verify357(set);
    }

    [Test]
    public void Set_ByCollectionExpression_2()
    {
        ImmSet<byte> set1 = [_3_, _5_, _7_];
        set1.ShouldBeAssignableTo<ImmListSet<byte>>();
        ImmListSet<byte> set2 = (set1 as ImmListSet<byte>)!;
        Verify357(set2);
    }

    [Test]
    public void SortedSet_ArgumentArray_Basic()
    {
        ImmListSet<byte> set = Imm.SortedSetOf(_7_, _3_, _5_);
        Verify357(set);
    }

    [Test]
    public void SortedSet_ArgumentArray_FromArray()
    {
        byte[]        bytes = [_7_, _3_, _5_];
        ImmListSet<byte> set  = Imm.SortedSetOf(bytes);
        Verify357(set);
    }

    [Test]
    public void SortedSet_ArgumentArray_FromArrayModified()
    {
        byte[]        bytes = [_7_, _3_, _5_];
        ImmListSet<byte> set  = Imm.SortedSetOf(bytes);
        Verify357(set);

        bytes[0] = 77;
        bytes[1] = 88;
        bytes[2] = 99;

        Verify357(set);
    }

    private static void Verify357(ImmListSet<byte> set)
    {
        set.Verify
        (
            s => s[0].ShouldBe(_3_),
            s => s[1].ShouldBe(_5_),
            s => s[2].ShouldBe(_7_),
            s => s.Imp.CascadingLevel.ShouldBe(_1_)
        );
    }



    private static IEnumerable<ulong> EnumerableMatch()
    {
        yield return 77uL;
        yield return 99uL;
        yield return 55uL;
        yield return 88uL;
        yield return 66uL;
    }

    private static IEnumerable<ulong> EnumerablePlus()
    {
        yield return 77uL;
        yield return 99uL;
        yield return 44uL;
        yield return 55uL;
        yield return 88uL;
        yield return 66uL;
    }

    private static IEnumerable<ulong> EnumerableMinus()
    {
        yield return 77uL;
        yield return 99uL;
        yield return 88uL;
        yield return 66uL;
    }

    private static IEnumerable<ulong> EnumerableOverlap()
    {
        yield return 77uL;
        yield return 99uL;
        yield return 70uL;
        yield return 66uL;
    }

    private static IEnumerable<ulong> EnumerableOther()
    {
        yield return 60uL;
        yield return 70uL;
        yield return 80uL;
        yield return 90uL;
        yield return 50uL;
    }


    [Test]
    public void Set_Easy_byCollectionExpression_1()
    {
        ImmSet<ulong> set = Imm.SetOf(55uL, 66uL, 77uL, 88uL, 99uL);
        Verify_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void Set_Easy_byCollectionExpression_2()
    {
        ImmSet<ulong> set = Imm.SetOf(55uL, 66uL, 77uL, 88uL, 99uL);
        Verify_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void Set_Easy_fromTrueEnumerable()
    {
        IEnumerable<ulong> source = EnumerableMatch();
        var                set    = source.ToImmSet();
        Verify_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void Set_Easy_fromEnumerableList()
    {
        IEnumerable<ulong> list = new List<ulong> { 88uL, 99uL, 66uL, 55uL, 77uL };
        var                set  = list.ToImmSet();
        Verify_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void Set_Easy_fromOrderedEnumerable()
    {
        IOrderedEnumerable<ulong> oe  = EnumerableMatch().Order();
        ImmListSet<ulong>      set = oe.ToImmSet();
        Verify_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void Set_Easy_fromArray()
    {
        ulong[] array = [55, 66, 77, 88, 99];
        var     set   = array.ToImmSet();
        Verify_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void Set_Easy_fromSpan()
    {
        ulong[]             array = [55, 66, 77, 88, 99];
        ReadOnlySpan<ulong> span  = array;
        var                 set   = span.ToImmSet();
        Verify_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void Set_Easy_fromCollection()
    {
        IReadOnlyCollection<ulong> collection = new List<ulong> { 77L, 55L, 99L, 88L, 66L };
        var set  = collection.ToImmSet();
        Verify_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void Set_Easy_fromList()
    {
        var list = new List<ulong> { 77L, 55L, 99L, 88L, 66L };
        var set  = list.ToImmSet();
        Verify_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void Set_Easy_fromHashSet()
    {
        var ss  = new HashSet<ulong> { 55L, 66L, 77L, 88L, 99L };
        var set = ss.ToImmSet();
        Verify_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void Set_Easy_fromSortedSet()
    {
        var ss  = new SortedSet<ulong> { 55L, 66L, 77L, 88L, 99L };
        var set = ss.ToImmSet();
        Verify_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void UnionSet_noIntersection()
    {
        ulong[]       arrayA   = [55uL, 88uL];
        ulong[]       arrayB   = [66uL, 77uL, 99uL];
        ImmSet<ulong> setA     = arrayA.ToImmSet();
        ImmSet<ulong> setB     = arrayB.ToImmSet();
        ImmSet<ulong> unionSet = new ImmutableUnionSet<ulong>(setA, setB);
        Verify_55_99(unionSet);
    }

    [Test]
    public void UnionSet_withIntersection()
    {
        ulong[]       arrayA   = [55uL, 77uL, 88uL];
        ulong[]       arrayB   = [66uL, 77uL, 99uL];
        ImmSet<ulong> setA     = arrayA.ToImmSet();
        ImmSet<ulong> setB     = arrayB.ToImmSet();
        ImmSet<ulong> unionSet = new ImmutableUnionSet<ulong>(setA, setB);
        Verify_55_99(unionSet);
    }

    [Test]
    public void UnionSet_fromPlus_TwoSets()
    {
        ulong[]       arrayA   = [55uL, 77uL, 88uL];
        ulong[]       arrayB   = [66uL, 77uL, 99uL];
        ImmSet<ulong> setA     = arrayA.ToImmSet();
        ImmSet<ulong> setB     = arrayB.ToImmSet();
        ImmSet<ulong> unionSet = setA + setB;
        Verify_55_99(unionSet);
    }

    [Test]
    public void UnionSet_fromPlus_SetAndArray()
    {
        ulong[]       arrayA   = [55uL, 77uL, 88uL];
        ulong[]       arrayB   = [66uL, 77uL, 99uL];
        ImmSet<ulong> setA     = arrayA.ToImmSet();
        ImmSet<ulong> unionSet = setA + arrayB;
        Verify_55_99(unionSet);
    }

    [Test]
    public void UnionSet_fromPlus_SetAndArrayThatIsNotNeeded()
    {
        ulong[]       arrayA   = [55uL, 66uL, 77uL, 88uL, 99uL];
        ulong[]       arrayB   = [66uL, 77uL, 88uL];
        ImmSet<ulong> setA     = arrayA.ToImmSet();
        ImmSet<ulong> unionSet = setA + arrayB;
        unionSet.ShouldBeSameAs(setA);
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
            x => x.Contains(100uL).ShouldBeFalse(),
            x => x.Overlaps(EnumerableMinus()).ShouldBeTrue(),
            x => x.Overlaps(EnumerablePlus()).ShouldBeTrue(),
            x => x.SetEquals(EnumerableMatch()).ShouldBeTrue()
        );


    [Test]
    public void SortedSet_Easy_byCollectionExpression_1()
    {
        ImmSortedListSet<ulong> set = [55uL, 66ul, 77uL, 88uL, 99uL];
        Verify_Sorted_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void SortedSet_Easy_byCollectionExpression_1s()
    {
        ImmSortedListSet<ulong> set = [77uL, 55uL, 99uL, 88uL, 66uL];
        Verify_Sorted_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void SortedSet_Easy_byCollectionExpression_2()
    {
        ImmSortedSet<ulong>     set1 = [55uL, 66ul, 77uL, 88uL, 99uL];
        ImmSortedListSet<ulong> set2 = (set1 as ImmSortedListSet<ulong>)!;
        Verify_Sorted_55_99(set2);
        VerifySets(set2);
    }

    [Test]
    public void SortedSet_Easy_fromArray()
    {
        ulong[] array = [77L, 55L, 99L, 88L, 66L];
        var     set   = array.ToImmSortedSet();
        Verify_Sorted_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void SortedSet_Easy_fromSpan()
    {
        ulong[]             array = [77L, 55L, 99L, 88L, 66L];
        ReadOnlySpan<ulong> span  = array.AsSpan();
        var                 set   = span.ToImmSortedSet();
        Verify_Sorted_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void SortedSet_Easy_fromCollection()
    {
        IReadOnlyCollection<ulong> collection = new List<ulong> { 77L, 55L, 99L, 88L, 66L };

        var set = collection.ToImmSortedSet();
        Verify_Sorted_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void SortedSet_Easy_fromReadOnlySet()
    {
        IReadOnlySet<ulong> ss  = new HashSet<ulong> { 55L, 66L, 77L, 88L, 99L };
        var set = ss.ToImmSortedSet();
        Verify_Sorted_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void SortedSet_Easy_fromSortedSet()
    {
        var ss  = new SortedSet<ulong> { 55L, 66L, 77L, 88L, 99L };
        var set = ss.ToImmSortedSet();
        Verify_Sorted_55_99(set);
        VerifySets(set);
    }

    [Test]
    public void SortedSet_Regular_fromArray()
    {
        ulong[] array = { 77L, 88L, 99L, 55L, 66L, 77L, 66L, 88L, 55L };
        var     set   = array.ToImmSortedSet();
        Verify_Sorted_55_99(set);
        VerifySets(set);
    }

    private void Verify_Sorted_55_99(ImmSortedListSet<ulong> set) =>
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
            x => x.SetEquals(EnumerableMatch()).ShouldBeTrue(),
            x => x.ToArray().ShouldBeEquivalentTo(new ulong[] {55, 66, 77, 88, 99})
        );


    private static void VerifySets(ImmSet<ulong> set) =>
        set.Verify
        (
            s => s.IsProperSupersetOf(EnumerableMinus()).ShouldBeTrue(),
            s => s.IsProperSupersetOf(EnumerableMatch()).ShouldBeFalse(),
            s => s.IsProperSupersetOf(EnumerablePlus()).ShouldBeFalse(),
            s => s.IsSupersetOf(EnumerableMinus()).ShouldBeTrue(),
            s => s.IsSupersetOf(EnumerableMatch()).ShouldBeTrue(),
            s => s.IsSupersetOf(EnumerablePlus()).ShouldBeFalse(),
            s => s.IsSubsetOf(EnumerableMinus()).ShouldBeFalse(),
            s => s.IsSubsetOf(EnumerableMatch()).ShouldBeTrue(),
            s => s.IsSubsetOf(EnumerablePlus()).ShouldBeTrue(),
            s => s.IsProperSubsetOf(EnumerableMinus()).ShouldBeFalse(),
            s => s.IsProperSubsetOf(EnumerableMatch()).ShouldBeFalse(),
            s => s.IsProperSubsetOf(EnumerablePlus()).ShouldBeTrue(),
            s => s.Overlaps(EnumerableMinus()).ShouldBeTrue(),
            s => s.Overlaps(EnumerableMatch()).ShouldBeTrue(),
            s => s.Overlaps(EnumerablePlus()).ShouldBeTrue(),
            s => s.Overlaps(EnumerableOverlap()).ShouldBeTrue(),
            s => s.Overlaps(EnumerableOther()).ShouldBeFalse(),
            s => s.SetEquals(EnumerableMatch()).ShouldBeTrue(),
            s => s.SetEquals(EnumerablePlus()).ShouldBeFalse(),
            s => s.SetEquals(EnumerableMinus()).ShouldBeFalse(),
            s => s.SetEquals(EnumerableOverlap()).ShouldBeFalse(),
            s => s.SetEquals(EnumerableOther()).ShouldBeFalse()
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

    private void VerifyEmptySortSet(ImmSortedListSet<ulong> set) =>
        set.Verify
        (
            x => x.IsEmpty.ShouldBeTrue(),
            x => x.IsNotEmpty.ShouldBeFalse(),
            x => x.Count.ShouldBe(0),
            x => x.IndexOf(1380L).ShouldBeNegative(),
            x => x.Contains(99L).ShouldBeFalse()
        );


}
