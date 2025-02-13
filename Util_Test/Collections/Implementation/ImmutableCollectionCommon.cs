namespace Util.Collections.Implementation;


[TestFixture]
public class ImmutableCollectionCommon
{

    #region CASE 1

    [Test]
    public void Case1_Singleton_Long()
    {
        var singleton = new ImmutableSingleton<long>(1984L);
        VerifyCase1<long>(singleton, 1984L, 9999L);
    }

    [Test]
    public void Case1_SortedSingleton_Long()
    {
        var singleton = new ImmutableSortedSingleton<long>(1984L);
        VerifyCase1<long>(singleton, 1984L, 9999L);
    }

    [Test]
    public void Case1_MiniSet_Long()
    {
        var set = new ImmutableMiniSet<long>(new [] {1984L});
        VerifyCase1<long>(set, 1984L, 9999L);
    }

    [Test]
    public void Case1_HashSet_Long()
    {
        var set = new ImmutableHashSet<long>(new [] {1984L});
        VerifyCase1<long>(set, 1984L, 9999L);
    }

    [Test]
    public void Case1_SortedSet_Long()
    {
        var set = new ImmutableSortedSet<long>(new [] {1984L});
        VerifyCase1<long>(set, 1984L, 9999L);
    }

    [Test]
    public void Case1_Singleton_StringM()
    {
        var set = new ImmutableSingleton<string>("Stone");
        VerifyCase1<string>(set, "Stone", "Water");
    }

    [Test]
    public void Case1_MiniSet_StringM()
    {
        var set = new ImmutableMiniSet<string>(new [] {"Stone"});
        VerifyCase1<string>(set, "Stone", "Water");
    }

    [Test]
    public void Case1_HashSet_StringM()
    {
        var set = new ImmutableHashSet<string>(new [] {"Stone"});
        VerifyCase1<string>(set, "Stone", "Water");
    }

    [Test]
    public void Case1_SortedSet_StringM()
    {
        var set = new ImmutableSortedSet<string>(new [] {"Stone"});
        VerifyCase1<string>(set, "Stone", "Water");
    }

    [Test]
    public void Case1_Singleton_StringNullable()
    {
        var set = new ImmutableSingleton<string?>("Stone");

        VerifyCase1<string?>(set, "Stone", null);

        set.IndexOf(null).ShouldBeNegative();
        set.Find(x => x is not null).Ok.ShouldBeTrue();
        set.Find(x => x is null).Ok.ShouldBeFalse();
    }

    [Test]
    public void Case1_MiniSet_StringNullable()
    {
        var set = new ImmutableMiniSet<string?>(new [] {"Stone"});

        VerifyCase1<string?>(set, "Stone", null);

        set.IndexOf(null).ShouldBeNegative();
        set.Find(x => x is not null).Ok.ShouldBeTrue();
        set.Find(x => x is null).Ok.ShouldBeFalse();
    }

    [Test]
    public void Case1_HashSet_StringNullable()
    {
        var set = new ImmutableHashSet<string?>(new [] {"Stone"});

        VerifyCase1<string?>(set, "Stone", null);

        set.IndexOf(null).ShouldBeNegative();
        set.Find(x => x is not null).Ok.ShouldBeTrue();
        set.Find(x => x is null).Ok.ShouldBeFalse();
    }

    [Test]
    public void Case1_Singleton_StringNullableNull()
    {
        string? str = null;
        var     set = new ImmutableSingleton<string?>(str);

        VerifyCase1<string?>(set, null, "nothing");

        set.IndexOf(null).ShouldBe(0);
        set.Find(x => x is null).Ok.ShouldBeTrue();
        set.Find(x => x is not null).Ok.ShouldBeFalse();
    }

    [Test]
    public void Case1_MiniSet_StringNullableNull()
    {
        string? str = null;
        var     set = new ImmutableMiniSet<string?>(new [] {str});

        VerifyCase1<string?>(set, null, "nothing");

        set.IndexOf(null).ShouldBe(0);
        set.Find(x => x is null).Ok.ShouldBeTrue();
        set.Find(x => x is not null).Ok.ShouldBeFalse();
    }

    [Test]
    public void Case1_HashSet_StringNullableNull()
    {
        string? str = null;
        var     set = new ImmutableHashSet<string?>(new [] {str});

        VerifyCase1<string?>(set, null, "nothing");

        set.IndexOf(null).ShouldBe(0);
        set.Find(x => x is null).Ok.ShouldBeTrue();
        set.Find(x => x is not null).Ok.ShouldBeFalse();
    }

    private static void VerifyCase1<T>(ImmList<T> set, T element, T nonexistentElement)
    {
        Verify
        (
            () => set.IsEmpty.ShouldBeFalse(),
            () => set.IsNotEmpty.ShouldBeTrue(),
            () => set.Count.ShouldBe(1),
            () => set.First.ShouldBe(element),
            () => set.Last.ShouldBe(element),
            () => set.At(0).ShouldBe(element),
            () => set[0].ShouldBe(element),
            () => set.IndexOf(element).ShouldBe(0),
            () => set.IndexOf(nonexistentElement, -33).ShouldBe(-33)
        );

        // ReSharper disable once GenericEnumeratorNotDisposed
        set.GetEnumerator().Verify
        (
            i => i.MoveNext().ShouldBeTrue(),
            i => i.Current.ShouldBe(element),
            i => i.MoveNext().ShouldBeFalse(),
            i => i.Dispose()
        );
    }

    #endregion


    #region CASE 3

    private static readonly long[] threeLongs = new long[] { 26L, 42L, 74L };

    [Test]
    public void Case3_List_Long()
    {
        var set = new ImmutableArrayList<long>(threeLongs);
        VerifyThreeLongs(set);
    }

    [Test]
    public void Case3_MiniSet_Long()
    {
        var set = new ImmutableMiniSet<long>(threeLongs);
        VerifyThreeLongs(set);
    }

    [Test]
    public void Case3_HashSet_Long()
    {
        var set = new ImmutableHashSet<long>(threeLongs);
        VerifyThreeLongs(set);
    }

    [Test]
    public void Case3_SortedSet_Long()
    {
        var set = new ImmutableSortedSet<long>(threeLongs);
        VerifyThreeLongs(set);
    }

    private static void VerifyThreeLongs(ImmutableArrayList<long> collection)
    {
        collection.Verify
        (
            c => c.IsNotEmpty.ShouldBeTrue(),
            c => c.IsEmpty.ShouldBeFalse(),
            c => c.Count.ShouldBe(3),
            c => c.First.ShouldBe(26L),
            c => c.Last.ShouldBe(74L),
            c => c.At(0).ShouldBe(26L),
            c => c.At(1).ShouldBe(42L),
            c => c.At(2).ShouldBe(74L),
            c => c[0].ShouldBe(26L),
            c => c[1].ShouldBe(42L),
            c => c[2].ShouldBe(74L),
            c => c.IndexOf(26L).ShouldBe(0),
            c => c.IndexOf(42L).ShouldBe(1),
            c => c.IndexOf(74L).ShouldBe(2),
            c => c.IndexOf(999L).ShouldBeNegative(),
            c => c.LastIndexOf(26L).ShouldBe(0),
            c => c.LastIndexOf(42L).ShouldBe(1),
            c => c.LastIndexOf(74L).ShouldBe(2),
            c => c.LastIndexOf(999L).ShouldBeNegative(),
            c => c.Contains(26L).ShouldBeTrue(),
            c => c.Contains(42L).ShouldBeTrue(),
            c => c.Contains(74L).ShouldBeTrue(),
            c => c.Contains(999L).ShouldBeFalse(),
            c => c.Find(x => x == 26L).Ok.ShouldBeTrue(),
            c => c.Find(x => x == 42L).Ok.ShouldBeTrue(),
            c => c.Find(x => x == 74L).Ok.ShouldBeTrue(),
            c => c.FindFirst(x => x > 1L).Item.ShouldBe(26L),
            c => c.FindFirst(x => x > 1L, 1).Item.ShouldBe(42L),
            c => c.FindFirst(x => x > 1L, 2).Item.ShouldBe(74L),
            c => c.FindFirst(x => x > 1L, 3).Ok.ShouldBeFalse(),
            c => c.FindLast(x => x <= 99L).Item.ShouldBe(74L),
            c => c.FindLast(x => x <= 33L).Item.ShouldBe(26L)
        );
    }

    #endregion

}
