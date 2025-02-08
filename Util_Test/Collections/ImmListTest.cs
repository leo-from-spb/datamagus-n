using System.Collections.Generic;

namespace Util.Collections;

[TestFixture]
public class ImmListTest
{
    [Test]
    public void Basic_ToImmList_FromArray()
    {
        var array = new[] { 26uL, 42uL, 74uL };
        var list  = array.ToImmList();
        BasicTest(list);
    }

    [Test]
    public void Basic_ToImmList_FromList()
    {
        List<ulong> netList = new List<ulong> { 26uL, 42uL, 74uL };
        ImmList<ulong> list = netList.ToImmList();
        netList.Clear();
        BasicTest(list);
    }

    [Test]
    public void Basic_ToImmList_FromImmList()
    {
        var array = new[] { 26uL, 42uL, 74uL };
        var list1 = array.ToImmList();
        var list2 = list1.ToImmList();
        BasicTest(list2);
        list2.ShouldBeSameAs(list1, "don't make a copy of an immutable list");
    }

    //[Test]
    //public void Basic_ToImmList_AsSlice()
    //{
    //    var array = new[] { 11uL, 26uL, 42uL, 74uL, 99uL };
    //    var list1 = array.ToImmList();
    //    var list2 = list1.Slice(1, 3);
    //    BasicTest(list2);
    //}

    private void BasicTest(ImmList<ulong> list) =>
        list.Verify
        (
            l => l.IsNotEmpty.ShouldBeTrue(),
            l => l.IsEmpty.ShouldBeFalse(),
            l => l.Count.ShouldBe(3),
            l => l.First.ShouldBe(26uL),
            l => l.Last.ShouldBe(74uL),
            l => l.At(0).ShouldBe(26uL),
            l => l.At(1).ShouldBe(42uL),
            l => l.At(2).ShouldBe(74uL),
            l => l.IndexOf(26uL).ShouldBe(0),
            l => l.IndexOf(42uL).ShouldBe(1),
            l => l.IndexOf(74uL).ShouldBe(2),
            l => l.IndexOf(99uL).ShouldBeNegative(),
            l => l.LastIndexOf(26uL).ShouldBe(0),
            l => l.LastIndexOf(42uL).ShouldBe(1),
            l => l.LastIndexOf(74uL).ShouldBe(2),
            l => l.LastIndexOf(99uL).ShouldBeNegative(),
            l => l.Contains(42uL).ShouldBeTrue(),
            l => l.Contains(66uL).ShouldBeFalse()
            //l => l.Slice(1, 1).First.ShouldBe(42uL),
            //l => l.Slice(0, 3).ShouldBeSameAs(l)
        );

}
