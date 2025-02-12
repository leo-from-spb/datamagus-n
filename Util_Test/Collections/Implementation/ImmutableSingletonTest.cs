namespace Util.Collections.Implementation;


public class ImmutableSingletonTest
{
    [Test]
    public void BasicLong()
    {
        var set = new ImmutableSortedSingleton<long>(1984L);
        VerifySingle<long>(set, 1984L, 9999L);
    }

    [Test]
    public void BasicStringNotNull()
    {
        var set = new ImmutableSingleton<string>("Stone");
        VerifySingle<string>(set, "Stone", "Water");
    }

    [Test]
    public void BasicStringNullable()
    {
        var set = new ImmutableSingleton<string?>("Stone");

        VerifySingle<string?>(set, "Stone", null);

        set.IndexOf(null).ShouldBeNegative();
        set.Find(x => x is not null).Ok.ShouldBeTrue();
        set.Find(x => x is null).Ok.ShouldBeFalse();
    }

    [Test]
    public void BasicStringNullableNull()
    {
        string? str = null;
        var     set = new ImmutableSingleton<string?>(str);

        VerifySingle<string?>(set, null, "nothing");

        set.IndexOf(null).ShouldBe(0);
        set.Find(x => x is null).Ok.ShouldBeTrue();
        set.Find(x => x is not null).Ok.ShouldBeFalse();
    }

    private static void VerifySingle<T>(ImmutableSingleton<T> set, T element, T nonexistentElement)
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
}
