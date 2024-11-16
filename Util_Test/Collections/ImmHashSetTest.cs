using System.Collections.Generic;
using System.Linq;

namespace Util.Collections;

[TestFixture]
public class ImmHashSetTest
{

    [Test]
    public void Basic()
    {
        ulong[] array = [12uL, 26uL, 42uL, 74uL, 99uL];
        ImmHashSet<ulong> set = new ImmHashSet<ulong>(array, true);
        BasicVerify(set);
    }

    [Test]
    public void Basic_ViaImm()
    {
        SortedSet<ulong> originalSet = [12uL, 26uL, 42uL, 74uL, 99uL];
        ImmSet<ulong> set = originalSet.ToImmSet();
        set.ShouldBeOfType<ImmHashSet<ulong>>();
        BasicVerify(set);
    }

    private static void BasicVerify(ImmSet<ulong> set)
    {
        set.Verify
        (
            s => s.Count.ShouldBe(5),
            s => s.At(0).ShouldBe(12ul),
            s => s.At(1).ShouldBe(26ul),
            s => s.At(2).ShouldBe(42ul),
            s => s.At(3).ShouldBe(74ul),
            s => s.At(4).ShouldBe(99ul),
            s => s.IndexOf(12uL).ShouldBe(0),
            s => s.IndexOf(26uL).ShouldBe(1),
            s => s.IndexOf(42uL).ShouldBe(2),
            s => s.IndexOf(74uL).ShouldBe(3),
            s => s.IndexOf(99uL).ShouldBe(4),
            s => s.IndexOf(1uL).ShouldBeNegative(),
            s => s.IndexOf(9999uL).ShouldBeNegative()
        );
    }


    [Test]
    public void Thousand()
    {
        ulong[] array = new ulong[1000];
        for (uint i = 0; i < 1000; i++) array[i] = 3uL * (i + 1) * (i + 1);
        array = array.Reverse().ToArray();
        ImmHashSet<ulong> set = new ImmHashSet<ulong>(array);

        for (int i = 0; i < 1000; i++)
        {
            ulong x = array[i];
            int index = set.IndexOf(x);
            index.ShouldBe(i);
        }
    }

    [Test]
    public void Thousand2()
    {
        ulong[] array = new ulong[1000];
        for (uint i = 0; i < 1000; i++) array[i] = 3uL * (i + 1) * (i + 1) | 0x8000_0000_0000_0000uL;
        ImmHashSet<ulong> set = new ImmHashSet<ulong>(array);

        for (int i = 0; i < 1000; i++)
        {
            ulong x = array[i];
            int index = set.IndexOf(x);
            index.ShouldBe(i);
        }
    }

    [Test]
    public void Million()
    {
        ulong[] array = new ulong[1000000];
        for (uint i = 0; i < 1000000; i++) array[i] = 7uL * (i + 1) * (i + 1);
        ImmHashSet<ulong> set = new ImmHashSet<ulong>(array);

        for (int i = 0; i < 1000000; i++)
        {
            ulong x = array[i];
            int index = set.IndexOf(x);
            index.ShouldBe(i);
        }
    }

}
