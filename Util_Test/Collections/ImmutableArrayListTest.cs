namespace Util.Collections;


[TestFixture]
public class ImmutableArrayListTest
{

    [Test]
    public void Basic()
    {
        long[] arr  = [55L, 77L, 99L];
        var    list = new ImmutableArrayList<long>(arr);

        Verify
        (
            () => list.Count.ShouldBe(3),
            () => list.IsNotEmpty.ShouldBeTrue(),
            () => list.IsEmpty.ShouldBeFalse(),
            () => list.First.ShouldBe(55L),
            () => list.Last.ShouldBe(99L),
            () => list.At(0).ShouldBe(55L),
            () => list.At(1).ShouldBe(77L),
            () => list.At(2).ShouldBe(99L),
            () => list.Contains(55L).ShouldBeTrue(),
            () => list.Contains(77L).ShouldBeTrue(),
            () => list.Contains(99L).ShouldBeTrue(),
            () => list.Contains(11L).ShouldBeFalse(),
            () => list.Find(x => x == 55L).ShouldBe(new Found<long>(true, 55L)),
            () => list.Find(x => x == 77L).ShouldBe(new Found<long>(true, 77L)),
            () => list.Find(x => x == 99L).ShouldBe(new Found<long>(true, 99L)),
            () => list.FindFirst(x => x == 55L, 0).ShouldBe(new Found<long>(true, 55L)),
            () => list.FindFirst(x => x == 55L, 1).ShouldBe(Found<long>.NotFound),
            () => list.FindFirst(x => x == 77L, 1).ShouldBe(new Found<long>(true, 77L)),
            () => list.FindLast(x => x == 55L).ShouldBe(new Found<long>(true, 55L)),
            () => list.FindLast(x => x == 77L).ShouldBe(new Found<long>(true, 77L)),
            () => list.IndexOf(55L).ShouldBe(0),
            () => list.IndexOf(77L).ShouldBe(1),
            () => list.IndexOf(99L).ShouldBe(2),
            () => list.LastIndexOf(55L).ShouldBe(0),
            () => list.LastIndexOf(77L).ShouldBe(1),
            () => list.LastIndexOf(99L).ShouldBe(2)
        );
    }


    [Test]
    public void Single()
    {
        long[] arr = [42L];
        var list = new ImmutableArrayList<long>(arr);

        Verify
        (
            () => list.Count.ShouldBe(1),
            () => list.IsNotEmpty.ShouldBeTrue(),
            () => list.IsEmpty.ShouldBeFalse(),
            () => list.First.ShouldBe(42L),
            () => list.Last.ShouldBe(42L),
            () => list.IndexOf(42L).ShouldBe(0),
            () => list.LastIndexOf(42L).ShouldBe(0)
        );
    }
}
