namespace Util.Collections.Implementation;

[TestFixture]
public class SortingLogicTest
{

    [Test]
    public void SortAndDeduplicate_Empty()
    {
        var array = SortingLogic.SortAndDeduplicate<ulong>([]);
        array.ShouldBeEmpty();
    }

    [Test]
    public void SortAndDeduplicate_SingleElement()
    {
        var array = SortingLogic.SortAndDeduplicate<ulong>([1974L]);
        array.ShouldBeEquivalentTo(new ulong[] { 1974L });
        array.Length.ShouldBe(1);
    }

    [Test]
    public void SortAndDeduplicate_AlreadySorted()
    {
        var array = SortingLogic.SortAndDeduplicate<ulong>([12, 26, 42, 74, 99]);
        array.ShouldBeEquivalentTo(new ulong[] {12, 26, 42, 74, 99});
        array.Length.ShouldBe(5);
    }

    [Test]
    public void SortAndDeduplicate_Sort_NoDuplicates()
    {
        var array = SortingLogic.SortAndDeduplicate<ulong>([7, 3, 10, 2, 9, 8, 1, 4, 6, 5]);
        array.ShouldBeEquivalentTo(new ulong[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
    }

    [Test]
    public void SortAndDeduplicate_Deduplicate_1()
    {
        var array = SortingLogic.SortAndDeduplicate<ulong>([11, 12, 13, 14, 15, 33, 33, 41, 42, 43, 44, 45]);
        array.ShouldBeEquivalentTo(new ulong[] { 11, 12, 13, 14, 15, 33, 41, 42, 43, 44, 45 });
    }

}
