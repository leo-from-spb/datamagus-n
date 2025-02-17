using System.Collections.Generic;
using System.Linq;
using Testing.Appliance.Assertions;
using Util.Collections.Implementation;

namespace Util.Collections;

[TestFixture]
public class ImmDictTest
{
    [Test]
    public void Basic1_Singleton()
    {
        var dict = new ImmutableSingletonDictionary<string,ulong>("thing", 42uL);
        VerifyBasic1(dict);
    }

    private static void VerifyBasic1(ImmDict<string,ulong> dict)
    {
        dict.Verify
        (
            d => d.IsNotEmpty.ShouldBeTrue(),
            d => d.IsEmpty.ShouldBeFalse(),
            d => d.Count.ShouldBe(1),
            d => d.Find("thing").ShouldBe(new Found<ulong>(true, 42L)),
            d => d.Find("nothing").ShouldBe(new Found<ulong>(false, 0)),
            d => d.ContainsKey("thing").ShouldBeTrue(),
            d => d.ContainsKey("nothing").ShouldBeFalse()
        );

        dict.Keys.Verify
        (
            ks => ks.IsNotEmpty.ShouldBeTrue(),
            ks => ks.IsEmpty.ShouldBeFalse(),
            ks => ks.Count.ShouldBe(1),
            ks => ks.Contains("thing").ShouldBeTrue(),
            ks => ks.Contains("nothing").ShouldBeFalse(),
            ks => (ks as IEnumerable<string>).ToArray().ShouldContainAll("thing")
        );

        dict.Values.Verify
        (
            vs => vs.IsNotEmpty.ShouldBeTrue(),
            vs => vs.IsEmpty.ShouldBeFalse(),
            vs => vs.Count.ShouldBe(1),
            vs => vs.Contains(42uL).ShouldBeTrue(),
            vs => vs.Contains(13uL).ShouldBeFalse(),
            vs => (vs as IEnumerable<ulong>).ToArray().ShouldContainAll(42uL)
        );
    }
}
