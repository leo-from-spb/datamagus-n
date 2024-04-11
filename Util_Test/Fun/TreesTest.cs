using System.Collections.Generic;
using System.Linq;
using Util.Common.Fun;
using static Util.Common.Fun.Trees;

namespace Util.Test.Fun;


[TestFixture]
public class TreesTest
{
    private SortedList<string, string[]> SimpleTree
        = new()
          {
              { "Germany", ["Bayern", "NRW"] },
              { "Bayern", ["München", "Augsburg"] },
              { "NRW", ["Köln", "Düsseldorf"] },
          };

    private readonly string[] EmptyStringArray = [];
    
    [Test]
    public void TraversDepthFirst_basic()
    {
        var list = 
            TraversDepthFirst("Germany", s => SimpleTree.Get(s, EmptyStringArray))
               .ToList();
        List<string> expected = ["Germany", "Bayern", "München", "Augsburg", "NRW", "Köln", "Düsseldorf"];
        list.ShouldBe(expected);
    }
    
    [Test]
    public void TraversBreadthFirst_basic()
    {
        var list = 
            TraverseBreadthFirst("Germany", s => SimpleTree.Get(s, EmptyStringArray))
               .ToList();
        List<string> expected = ["Germany", "Bayern", "NRW", "München", "Augsburg", "Köln", "Düsseldorf"];
        list.ShouldBe(expected);
    }
    
}
