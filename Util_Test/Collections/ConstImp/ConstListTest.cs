using System.Collections.Generic;
using Util.Extensions;

namespace Util.Collections.ConstImp;


[TestFixture]
public class ConstListTest
{

    [Test, Order(10)]
    public void Basic()
    {
        ConstArrayList<uint> list = new ConstArrayList<uint>(new uint[] { 26u, 13u, 88u, 74u });
        list.Verify
        (
            l => l.IsNotEmpty.ShouldBeTrue(),
            l => l.IsEmpty.ShouldBeFalse(),
            l => l.Count.ShouldBe(4),
            l => l.First.ShouldBe(26u),
            l => l.Last.ShouldBe(74u),
            l => l.At(2).ShouldBe(88u),
            l => l[2].ShouldBe(88u)
        );
    }

    [Test, Order(11)]
    public void Basic_R()
    {
        RList<uint> list = new ConstArrayList<uint>(new uint[] { 26u, 13u, 88u, 74u });
        list.Verify
        (
            l => l.IsNotEmpty.ShouldBeTrue(),
            l => l.IsEmpty.ShouldBeFalse(),
            l => l.Count.ShouldBe(4),
            l => l.First.ShouldBe(26u),
            l => l.Last.ShouldBe(74u),
            l => l.At(2).ShouldBe(88u),
            l => l[2].ShouldBe(88u)
        );
    }

    [Test, Order(12)]
    public void Basic_L()
    {
        LList<uint> list = new ConstArrayList<uint>(new uint[] { 26u, 13u, 88u, 74u });
        list.Verify
        (
            l => l.IsNotEmpty.ShouldBeTrue(),
            l => l.IsEmpty.ShouldBeFalse(),
            l => l.Count.ShouldBe(4),
            l => l.First.ShouldBe(26u),
            l => l.Last.ShouldBe(74u),
            l => l.At(2).ShouldBe(88u),
            l => l[2].ShouldBe(88u)
        );
    }

    [Test, Order(30)]
    public void Search()
    {
        ConstArrayList<uint> list = new ConstArrayList<uint>(new uint[] { 1u, 44u, 3u, 44u, 5u });
        list.Verify
        (
            l => l.IndexOf(44u).ShouldBe(1),
            l => l.IndexOf(x => x == 44u).ShouldBe(1),
            l => l.LastIndexOf(44u).ShouldBe(3),
            l => l.LastIndexOf(x => x == 44u).ShouldBe(3),
            l => l.Contains(44u).ShouldBeTrue(),
            l => l.Contains(x => x == 44u).ShouldBeTrue()
        );
    }

    [Test, Order(31)]
    public void Search_R()
    {
        RList<uint> list = new ConstArrayList<uint>(new uint[] { 1u, 44u, 3u, 44u, 5u });
        list.Verify
        (
            l => l.IndexOf(44u).ShouldBe(1),
            l => l.IndexOf(x => x == 44u).ShouldBe(1),
            l => l.LastIndexOf(44u).ShouldBe(3),
            l => l.LastIndexOf(x => x == 44u).ShouldBe(3),
            l => l.Contains(44u).ShouldBeTrue(),
            l => l.Contains(x => x == 44u).ShouldBeTrue()
        );
    }

    [Test, Order(32)]
    public void Search_L()
    {
        LList<uint> list = new ConstArrayList<uint>(new uint[] { 1u, 44u, 3u, 44u, 5u });
        list.Verify
        (
            l => l.IndexOf(x => x == 44u).ShouldBe(1),
            l => l.LastIndexOf(x => x == 44u).ShouldBe(3),
            l => l.Contains(x => x == 44u).ShouldBeTrue()
        );
    }

    [Test, Order(40)]
    public void Search_notFound()
    {
        ConstArrayList<uint> list = new ConstArrayList<uint>(new uint[] { 1u, 22u, 333u, 4444u });
        list.Verify
        (
            l => l.IndexOf(100u).ShouldBeNegative(),
            l => l.IndexOf(100u, -33).ShouldBe(-33),
            l => l.IndexOf(x => x == 200u).ShouldBeNegative(),
            l => l.IndexOf(x => x == 200u, -77).ShouldBe(-77),
            l => l.LastIndexOf(100u).ShouldBeNegative(),
            l => l.LastIndexOf(100u, -33).ShouldBe(-33),
            l => l.LastIndexOf(x => x == 200u).ShouldBeNegative(),
            l => l.LastIndexOf(x => x == 200u, -77).ShouldBe(-77),
            l => l.Contains(66u).ShouldBeFalse(),
            l => l.Contains(x => x == 66u).ShouldBeFalse()
        );
    }

    [Test, Order(41)]
    public void Search_notFound_R()
    {
        RList<uint> list = new ConstArrayList<uint>(new uint[] { 1u, 22u, 333u, 4444u });
        list.Verify
        (
            l => l.IndexOf(100u).ShouldBeNegative(),
            l => l.IndexOf(100u, -33).ShouldBe(-33),
            l => l.IndexOf(x => x == 200u).ShouldBeNegative(),
            l => l.IndexOf(x => x == 200u, -77).ShouldBe(-77),
            l => l.LastIndexOf(100u).ShouldBeNegative(),
            l => l.LastIndexOf(100u, -33).ShouldBe(-33),
            l => l.LastIndexOf(x => x == 200u).ShouldBeNegative(),
            l => l.LastIndexOf(x => x == 200u, -77).ShouldBe(-77),
            l => l.Contains(66u).ShouldBeFalse(),
            l => l.Contains(x => x == 66u).ShouldBeFalse()
        );
    }

    [Test, Order(42)]
    public void Search_notFound_L()
    {
        LList<uint> list = new ConstArrayList<uint>(new uint[] { 1u, 22u, 333u, 4444u });
        list.Verify
        (
            l => l.IndexOf(x => x == 200u).ShouldBeNegative(),
            l => l.IndexOf(x => x == 200u, -77).ShouldBe(-77),
            l => l.LastIndexOf(x => x == 300u).ShouldBeNegative(),
            l => l.LastIndexOf(x => x == 300u, -77).ShouldBe(-77),
            l => l.Contains(x => x == 66u).ShouldBeFalse()
        );
    }


    [Test, Order(50)]
    public void Segment()
    {
        uint[]        array = [11u, 22u, 33u, 44u, 55u, 66u, 77u, 88u];
        ConstArrayList<uint> list  = new ConstArrayList<uint>(array, 2, 6, false);
        list.Verify
        (
            l => l.Count.ShouldBe(4),
            l => l.First.ShouldBe(33u),
            l => l.Last.ShouldBe(66u),
            l => l[0].ShouldBe(33u),
            l => l[1].ShouldBe(44u),
            l => l[2].ShouldBe(55u),
            l => l[3].ShouldBe(66u)
        );
    }

    [Test, Order(51)]
    public void Segment_Search()
    {
        uint[]        array = [11u, 22u, 33u, 44u, 55u, 66u, 77u, 88u];
        ConstArrayList<uint> list  = new ConstArrayList<uint>(array, 2, 6, false);
        list.Verify
        (
            l => l.IndexOf(11u).ShouldBeNegative(),
            l => l.IndexOf(22u).ShouldBeNegative(),
            l => l.IndexOf(33u).ShouldBe(0),
            l => l.IndexOf(66u).ShouldBe(3),
            l => l.IndexOf(77u).ShouldBeNegative(),
            l => l.IndexOf(88u).ShouldBeNegative(),
            l => l.IndexOf(x => x == 11u).ShouldBeNegative(),
            l => l.IndexOf(x => x == 22u).ShouldBeNegative(),
            l => l.IndexOf(x => x == 33u).ShouldBe(0),
            l => l.IndexOf(x => x == 66u).ShouldBe(3),
            l => l.IndexOf(x => x == 77u).ShouldBeNegative(),
            l => l.IndexOf(x => x == 88u).ShouldBeNegative(),
            l => l.LastIndexOf(11u).ShouldBeNegative(),
            l => l.LastIndexOf(22u).ShouldBeNegative(),
            l => l.LastIndexOf(33u).ShouldBe(0),
            l => l.LastIndexOf(66u).ShouldBe(3),
            l => l.LastIndexOf(77u).ShouldBeNegative(),
            l => l.LastIndexOf(88u).ShouldBeNegative(),
            l => l.LastIndexOf(x => x == 11u).ShouldBeNegative(),
            l => l.LastIndexOf(x => x == 22u).ShouldBeNegative(),
            l => l.LastIndexOf(x => x == 33u).ShouldBe(0),
            l => l.LastIndexOf(x => x == 66u).ShouldBe(3),
            l => l.LastIndexOf(x => x == 77u).ShouldBeNegative(),
            l => l.LastIndexOf(x => x == 88u).ShouldBeNegative(),
            l => l.Contains(22u).ShouldBeFalse(),
            l => l.Contains(33u).ShouldBeTrue(),
            l => l.Contains(66u).ShouldBeTrue(),
            l => l.Contains(77u).ShouldBeFalse(),
            l => l.Contains(x => x == 22u).ShouldBeFalse(),
            l => l.Contains(x => x == 33u).ShouldBeTrue(),             
            l => l.Contains(x => x == 66u).ShouldBeTrue(),
            l => l.Contains(x => x == 77u).ShouldBeFalse()
        );
    }

    [Test, Order(50)]
    public void Segment_Iteration()
    {
        uint[] array = [11u, 22u, 33u, 44u, 55u, 66u, 77u, 88u];

        IEnumerable<uint> list = new ConstArrayList<uint>(array, 2, 6, false);

        string iterated = list.JoinToString(func: x => x.ToString());

        iterated.ShouldBe("33, 44, 55, 66");
    }


}
