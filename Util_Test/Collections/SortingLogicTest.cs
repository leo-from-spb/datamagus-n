namespace Util.Collections;

[TestFixture]
public class SortingLogicTest
{

    [Test]
    public void SortAndDeduplicate_Empty()
    {
        var arraySegment = SortingLogic.SortAndDeduplicate<ulong>([]);
        arraySegment.ShouldBeEmpty();
    }

    [Test]
    public void SortAndDeduplicate_SingleElement()
    {
        var arraySegment = SortingLogic.SortAndDeduplicate<ulong>([1974L]);
        arraySegment.Array.ShouldBeEquivalentTo(new ulong[] { 1974L });
        arraySegment.Offset.ShouldBe(0);
        arraySegment.Count.ShouldBe(1);
    }

    [Test]
    public void SortAndDeduplicate_AlreadySorted()
    {
        var arraySegment = SortingLogic.SortAndDeduplicate<ulong>([12, 26, 42, 74, 99]);
        arraySegment.Array.ShouldBeEquivalentTo(new ulong[] {12, 26, 42, 74, 99});
        arraySegment.Offset.ShouldBe(0);
        arraySegment.Count.ShouldBe(5);
    }

    [Test]
    public void SortAndDeduplicate_Sort_NoDuplicates()
    {
        var arraySegment = SortingLogic.SortAndDeduplicate<ulong>([7, 3, 10, 2, 9, 8, 1, 4, 6, 5]);
        var newArray     = arraySegment.ToArray();
        newArray.ShouldBeEquivalentTo(new ulong[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
    }

    [Test]
    public void SortAndDeduplicate_Deduplicate_1()
    {
        var arraySegment = SortingLogic.SortAndDeduplicate<ulong>([11, 12, 13, 14, 15, 33, 33, 41, 42, 43, 44, 45]);
        var newArray     = arraySegment.ToArray();
        newArray.ShouldBeEquivalentTo(new ulong[] { 11, 12, 13, 14, 15, 33, 41, 42, 43, 44, 45 });

    }

}
