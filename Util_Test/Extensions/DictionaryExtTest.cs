using System.Collections.Generic;

namespace Util.Extensions;

[TestFixture]
public class DictionaryExtTest
{

    [Test]
    public void Get_ValueType()
    {
        var dict = new Dictionary<string, long>()
                   {
                       { "A", 13L },
                       { "B", 44L}
                   };
        dict.Verify
        (
            d => d.Get("A").ShouldBe(13L),
            d => d.Get("B", -100L).ShouldBe(44L),
            d => d.Get("X", -100L).ShouldBe(-100L),
            d => d.Get("Z").ShouldBe(0L)
            );
    }

    [Test]
    public void Get_RefType()
    {
        var dict = new Dictionary<string, string>()
                   {
                       { "A", "One" },
                       { "B", "Two"}
                   };
        dict.Verify
        (
            d => d.Get("A").ShouldBe("One"),
            d => d.Get("B", "mura").ShouldBe("Two"),
            d => d.Get("X", "nothing").ShouldBe("nothing"),
            d => d.Get("Z").ShouldBeNull()
            );
    }

}
