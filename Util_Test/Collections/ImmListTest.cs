namespace Util.Collections;


[TestFixture]
public class ImmListTest
{

    [Test, Order(10)]
    public void Basic()
    {
        ImmList<uint> list = new ImmList<uint>(new uint[] { 26u, 13u, 88u, 74u });
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
        RList<uint> list = new ImmList<uint>(new uint[] { 26u, 13u, 88u, 74u });
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
        LList<uint> list = new ImmList<uint>(new uint[] { 26u, 13u, 88u, 74u });
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


    [Test, Order(20)]
    public void Search()
    {
        ImmList<uint> list = new ImmList<uint>(new uint[] { 1u, 44u, 3u, 44u, 5u });
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

    [Test, Order(21)]
    public void Search_R()
    {
        RList<uint> list = new ImmList<uint>(new uint[] { 1u, 44u, 3u, 44u, 5u });
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

    [Test, Order(22)]
    public void Search_L()
    {
        LList<uint> list = new ImmList<uint>(new uint[] { 1u, 44u, 3u, 44u, 5u });
        list.Verify
        (
            l => l.IndexOf(x => x == 44u).ShouldBe(1),
            l => l.LastIndexOf(x => x == 44u).ShouldBe(3),
            l => l.Contains(x => x == 44u).ShouldBeTrue()
        );
    }

    [Test, Order(30)]
    public void Search_notFound()
    {
        ImmList<uint> list = new ImmList<uint>(new uint[] { 1u, 22u, 333u, 4444u });
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

    [Test, Order(31)]
    public void Search_notFound_R()
    {
        RList<uint> list = new ImmList<uint>(new uint[] { 1u, 22u, 333u, 4444u });
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

    [Test, Order(32)]
    public void Search_notFound_L()
    {
        LList<uint> list = new ImmList<uint>(new uint[] { 1u, 22u, 333u, 4444u });
        list.Verify
        (
            l => l.IndexOf(x => x == 200u).ShouldBeNegative(),
            l => l.IndexOf(x => x == 200u, -77).ShouldBe(-77),
            l => l.LastIndexOf(x => x == 300u).ShouldBeNegative(),
            l => l.LastIndexOf(x => x == 300u, -77).ShouldBe(-77),
            l => l.Contains(x => x == 66u).ShouldBeFalse()
        );
    }
    
}
