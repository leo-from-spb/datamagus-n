using System;
using System.Collections.Generic;
using Util.Extensions;

namespace Util.Collections;

// ReSharper disable InconsistentNaming
// ReSharper disable UseCollectionExpression
#pragma warning disable CA1825


[TestFixture]
public class ImmTest
{

    private static readonly ulong[] emptyArrayOfULong = new ulong[0];

    private static readonly ulong[] singletonArray_26 = { 26uL };

    private static readonly ulong[] basicArray_26_44_74 = { 26uL, 44uL, 74uL };


    #region ListOf

    [Test] public void ListOf_Basic_0_Empty() => ListOf_Basic_0_Verify(Imm.EmptySet<ulong>());
    [Test] public void ListOf_Basic_0_Array() => ListOf_Basic_0_Verify(Imm.ListOf(emptyArrayOfULong));
    [Test] public void ListOf_Basic_0_Param() => ListOf_Basic_0_Verify(Imm.ListOf<ulong>());
    [Test] public void ListOf_Basic_0_Span()  => ListOf_Basic_0_Verify(new Span<ulong>().ToImmList());

    private static void ListOf_Basic_0_Verify(ImmList<ulong> list) =>
        list.Verify
        (
            l => l.ShouldBeEmpty(),
            l => l.ShouldBeOfType<ConstEmptySet<ulong>>()
        );


    [Test] public void ListOf_Basic_1_Array() => ListOf_Basic_1_Verify(Imm.ListOf(singletonArray_26));
    [Test] public void ListOf_Basic_1_Param() => ListOf_Basic_1_Verify(Imm.ListOf(26uL));
    [Test] public void ListOf_Basic_1_MSpan() => ListOf_Basic_1_Verify(singletonArray_26.AsSpan().ToImmList());
    [Test] public void ListOf_Basic_1_RSpan() => ListOf_Basic_1_Verify(singletonArray_26.AsReadOnlySpan().ToImmList());

    private static void ListOf_Basic_1_Verify(ImmList<ulong> list) =>
        list.Verify
        (
            l => l.ShouldNotBeEmpty(),
            l => l.First.ShouldBe(26uL),
            l => l.Last.ShouldBe(26uL),
            l => l.ShouldBeOfType<ConstSingletonSet<ulong>>()
        );


    [Test] public void ListOf_Basic_3_Array() => ListOf_Basic_3_Verify(Imm.ListOf(basicArray_26_44_74));
    [Test] public void ListOf_Basic_3_Param() => ListOf_Basic_3_Verify(Imm.ListOf(26uL, 44uL, 74uL));
    [Test] public void ListOf_Basic_3_MSpan() => ListOf_Basic_3_Verify(basicArray_26_44_74.AsSpan().ToImmList());
    [Test] public void ListOf_Basic_3_RSpan() => ListOf_Basic_3_Verify(basicArray_26_44_74.AsReadOnlySpan().ToImmList());

    private static void ListOf_Basic_3_Verify(ImmList<ulong> list) =>
        list.Verify
        (
            l => l.ShouldNotBeEmpty(),
            l => l.Count.ShouldBe(3),
            l => l.First.ShouldBe(26uL),
            l => l[0].ShouldBe(26uL),
            l => l[1].ShouldBe(44uL),
            l => l[2].ShouldBe(74uL),
            l => l.Last.ShouldBe(74uL),
            l => l.ShouldBeOfType<ConstArrayList<ulong>>()
        );

    #endregion


    #region SortedSetOf

    [Test] public void SortedSetOf_0_Empty() => SortedSetOf_0_Verify(Imm.EmptySortedSet<ulong>());
    [Test] public void SortedSetOf_0_Array() => SortedSetOf_0_Verify(Imm.SortedSetOf(emptyArrayOfULong));
    [Test] public void SortedSetOf_0_Param() => SortedSetOf_0_Verify(Imm.SortedSetOf<ulong>());
    [Test] public void SortedSetOf_0_RSpan()  => SortedSetOf_0_Verify(emptyArrayOfULong.AsReadOnlySpan().ToImmSortedSet());
    [Test] public void SortedSetOf_0_IEnum() => SortedSetOf_0_Verify((emptyArrayOfULong as IEnumerable<ulong>).ToImmSortedSet());

    private static void SortedSetOf_0_Verify(ImmSortedSet<ulong> set) =>
        set.Verify
        (
            s => s.ShouldBeEmpty(),
            s => s.Count.ShouldBe(0),
            s => s.ShouldBeOfType<ConstEmptySortedSet<ulong>>()
        );

    [Test] public void SortedSetOf_Basic_1_Array() => SortedSetOf_Basic_1_Verify(Imm.SortedSetOf(singletonArray_26));
    [Test] public void SortedSetOf_Basic_1_Param() => SortedSetOf_Basic_1_Verify(Imm.SortedSetOf(26uL));
    [Test] public void SortedSetOf_Basic_1_MSpan() => SortedSetOf_Basic_1_Verify(singletonArray_26.AsSpan().ToImmSortedSet());
    [Test] public void SortedSetOf_Basic_1_RSpan() => SortedSetOf_Basic_1_Verify(singletonArray_26.AsReadOnlySpan().ToImmSortedSet());
    [Test] public void SortedSetOf_Basic_1_IEnum() => SortedSetOf_Basic_1_Verify((singletonArray_26 as IEnumerable<ulong>).ToImmSortedSet());

    [Test] public void SortedSetOf_Basic_1_DeDup() => SortedSetOf_Basic_1_Verify(Imm.SortedSetOf(26uL, 26uL));

    private static void SortedSetOf_Basic_1_Verify(ImmList<ulong> list) =>
        list.Verify
        (
            l => l.ShouldNotBeEmpty(),
            l => l.Count.ShouldBe(1),
            l => l.First.ShouldBe(26uL),
            l => l.Last.ShouldBe(26uL),
            l => l.ShouldContain(26uL),
            l => l.ShouldBeOfType<ConstSingletonSortedSet<ulong>>()
        );

    [Test] public void SortedSetOf_Basic_Array() => SortedSetOf_3_Verify(Imm.SortedSetOf(basicArray_26_44_74));
    [Test] public void SortedSetOf_Basic_Param() => SortedSetOf_3_Verify(Imm.SortedSetOf(26uL, 44uL, 74uL));
    [Test] public void SortedSetOf_Basic_MSpan() => SortedSetOf_3_Verify(basicArray_26_44_74.AsSpan().ToImmSortedSet());
    [Test] public void SortedSetOf_Basic_RSpan() => SortedSetOf_3_Verify(basicArray_26_44_74.AsReadOnlySpan().ToImmSortedSet());

    [Test] public void SortedSetOf_Sort()        => SortedSetOf_3_Verify(Imm.SortedSetOf(74uL, 26uL, 44uL));
    [Test] public void SortedSetOf_Deduplicate() => SortedSetOf_3_Verify(Imm.SortedSetOf(26uL, 26uL, 26uL, 44uL, 44uL, 74uL, 74uL, 74uL));
    [Test] public void SortedSetOf_UniqueSort()  => SortedSetOf_3_Verify(Imm.SortedSetOf(44uL, 26uL, 74uL, 44uL, 26uL, 74uL, 44uL, 26uL));

    private static void SortedSetOf_3_Verify(ImmSortedSet<ulong> set) =>
        set.Verify
        (
            s => s.ShouldNotBeEmpty(),
            s => s.Count.ShouldBe(3),
            l => l.First.ShouldBe(26uL),
            l => l[0].ShouldBe(26uL),
            l => l[1].ShouldBe(44uL),
            l => l[2].ShouldBe(74uL),
            l => l.Last.ShouldBe(74uL),
            s => s.ShouldBeOfType<ConstArraySortedSet<ulong>>()
        );

    [Test]
    public void SortedSetOf_CopiesTheArray()
    {
        string[] originalStrings = { "First", "Second", "Third" };

        ImmSortedSet<string> set = Imm.SortedSetOf(originalStrings);

        set.Verify(
            s => s[0].ShouldBe("First"),
            s => s[1].ShouldBe("Second"),
            s => s[2].ShouldBe("Third")
        );

        originalStrings[0] = "Mura";
        originalStrings[1] = "Labuda";
        originalStrings[2] = "Something Weird";

        set.Verify(
            s => s[0].ShouldBe("First"),
            s => s[1].ShouldBe("Second"),
            s => s[2].ShouldBe("Third")
        );
    }

    #endregion



    #region Dictionary

    [Test]
    public void Dictionary_Imm_Basic_0()
    {
        var origin = new Dictionary<ulong, string>();

        var immD = origin.ToImm();
        immD.Verify
        (
            d => d.IsEmpty.ShouldBeTrue(),
            d => d.Count.ShouldBe(0),
            d => d.ShouldBeOfType<ImmEmptyDictionary<ulong, string>>()
        );
    }

    [Test]
    public void Dictionary_Imm_Basic_1()
    {
        var origin = new Dictionary<ulong, string>
                     {
                         { 5uL, "fünf" }
                     };


        var immD = origin.ToImm();
        immD.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.Count.ShouldBe(1),
            d => d.ShouldBeOfType<ImmMicroDictionary<ulong, string>>(),
            d => d[5uL].ShouldBe("fünf")
        );
    }

    [Test]
    public void Dictionary_Imm_Basic_3()
    {
        var origin = new Dictionary<ulong, string>
                     {
                         { 1uL, "einz" },
                         { 2uL, "zwei" },
                         { 3uL, "drei" },
                     };

        var immD = origin.ToImm();
        immD.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.Count.ShouldBe(3),
            d => d.ShouldBeOfType<ImmMicroDictionary<ulong, string>>(),
            d => d[1uL].ShouldBe("einz"),
            d => d[2uL].ShouldBe("zwei"),
            d => d[3uL].ShouldBe("drei")
        );
    }

    [Test]
    public void Dictionary_Imm_Basic_N()
    {
        const int N = 20;

        var origin = new Dictionary<ulong, string>(N);

        for (int i = 1; i <= N; i++) origin[(ulong)i] = i.ToString();

        var immD = origin.ToImm();
        immD.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.Count.ShouldBe(N),
            d => d.ShouldBeOfType<ImmCompactHashDictionary<ulong, string>>(),
            d => d[3uL].ShouldBe("3")
        );
    }


    [Test]
    public void R_InterfaceMethods()
    {
        ROrderDictionary<ulong, string> dictionary =
            Imm.Dictionary<ulong, string>(26uL, "Not 42", 42uL, "This is 42");
        dictionary.Verify
        (
            d => d[0].Key.ShouldBe(26uL),
            d => d[1].Key.ShouldBe(42uL),
            d => d.Find(26uL).Item.ShouldBe("Not 42"),
            d => d.Get(26uL).ShouldBe("Not 42"),
            d => d[26uL].ShouldBe("Not 42"),
            d => d[42uL].ShouldBe("This is 42"),
            d => d.ContainsKey(26uL).ShouldBeTrue(),
            d => d.Count.ShouldBe(2),
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.IsEmpty.ShouldBeFalse(),
            d => d.Keys.ShouldContain(26uL),
            d => d.Values.ShouldContain("Not 42"),
            d => d.Entries.ShouldNotBeEmpty()
        );
    }

    [Test]
    public void I_InterfaceMethods_IReadOnlyDictionary()
    {
        IReadOnlyDictionary<ulong, string> dictionary = Imm.Dictionary<ulong, string>(26uL, "Not 42");
        dictionary.Verify
        (
            d => d.TryGetValue(26uL, out _).ShouldBeTrue(),
            d => d[26uL].ShouldBe("Not 42"),
            d => d.ContainsKey(26uL).ShouldBeTrue(),
            d => d.Count.ShouldBe(1),
            d => d.Keys.ShouldContain(26uL),
            d => d.Values.ShouldContain("Not 42"),
            d => d.GetEnumerator().Dispose()
        );
    }

    #endregion
}
