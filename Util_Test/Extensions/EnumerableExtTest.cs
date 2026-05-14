using System.Collections.Generic;
using System.Linq;

namespace Util.Extensions;

[TestFixture]
public class EnumerableExtTest
{
    
    [Test]
    public void SelectNotNull_Basic()
    {
        List<string?> source =
            ["first", null, "second", null, null, "third", null];
        List<string> result =
            source.SelectNotNull(s => s?.ToUpper())
                  .ToList();
        List<string> expected =
            ["FIRST", "SECOND", "THIRD"];

        result.ShouldBe(expected);
    }

}
