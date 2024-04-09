using Util.Common.Fun;
using static Util.Common.Fun.NumberConstants;

namespace Util.Test.Fun;


[TestFixture]
public class DictionaryExtTest
{

    [Test]
    public void IDictionary_Get_byRef()
    {
        IDictionary<string, string> dict =
            new Dictionary<string, string> { {"First", "One"}, {"Second", "Two"} };

        Verify
        (
            () => dict.Get("First").ShouldBe("One"),
            () => dict.Get("Second").ShouldBe("Two"),
            () => dict.Get("Third").ShouldBeNull()
        );
    }

    [Test]
    public void IDictionary_Get_byValue()
    {
        IDictionary<string, byte> dict =
            new Dictionary<string, byte> { {"First", _1_}, {"Second", _2_} };

        Verify
        (
            () => dict.Get("First").ShouldBe(_1_),
            () => dict.Get("Second").ShouldBe(_2_),
            () => dict.Get("Third").ShouldBe(_0_)
        );
    }

}
