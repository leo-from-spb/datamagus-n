using Testing.Appliance.Assertions;
using Util.Extensions;
using static Util.Collections.ConstImp.ConstCollectionAlgorithm;

namespace Util.Collections.ConstImp;


[TestFixture]
public class ConstCollectionAlgorithmTest
{

    [Test]
    public void SortUnique_AllDiff()
    {
        int[] arr = [44, 33, 11, 22, 55];
        SortUnique(arr, out var n);

        arr.ShouldContainExactly(11, 22, 33, 44, 55);
        n.ShouldBe(5);
    }


    [Test]
    public void SortUnique_Diffs_1()
    {
        int[] arr = [11, 25, 25, 25, 25, 40];
        SortUnique(arr, out var n);

        arr.Segment(0, n).ShouldContainExactly(11, 25, 40);
        n.ShouldBe(3);
    }

    [Test]
    public void SortUnique_Diffs_2()
    {
        int[] arr = [11, 25, 25, 25, 25, 40, 77, 77, 77, 77, 90];
        SortUnique(arr, out var n);

        arr.Segment(0, n).ShouldContainExactly(11, 25, 40, 77, 90);
        n.ShouldBe(5);
    }

    [Test]
    public void SortUnique_Diffs_3()
    {
        int[] arr = [11, 25, 25, 25, 25, 77, 77, 77, 77, 90];
        SortUnique(arr, out var n);

        arr.Segment(0, n).ShouldContainExactly(11, 25, 77, 90);
        n.ShouldBe(4);
    }

    [Test]
    public void SortUnique_Diffs_4()
    {
        int[] arr = [25, 25, 25, 25, 77, 77, 77, 77];
        SortUnique(arr, out var n);

        arr.Segment(0, n).ShouldContainExactly(25, 77);
        n.ShouldBe(2);
    }


}
