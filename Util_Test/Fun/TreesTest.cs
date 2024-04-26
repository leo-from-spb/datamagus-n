using System.Collections.Generic;
using System.Linq;
using Util.Extensions;
using static Util.Fun.Trees;

namespace Util.Fun;


[TestFixture]
public class TreesTest
{
    private SortedList<string, string[]> SimpleTree
        = new()
          {
              { "Germany", ["Bayern", "NRW"] },
              { "Bayern", ["München", "Augsburg"] },
              { "München", ["Laim", "Pasing"] },
              { "NRW", ["Köln", "Düsseldorf"] },
          };

    private readonly string[] EmptyStringArray = [];
    
    [Test]
    public void TraversDepthFirst_basic()
    {
        var list = 
            TraversDepthFirst("Germany", s => SimpleTree.Get(s, EmptyStringArray))
               .ToList();
        List<string> expected = ["Germany", "Bayern", "München", "Laim", "Pasing", "Augsburg", "NRW", "Köln", "Düsseldorf"];
        list.ShouldBe(expected);
    }
    
    [Test]
    public void TraversBreadthFirst_basic()
    {
        var list = 
            TraverseBreadthFirst("Germany", s => SimpleTree.Get(s, EmptyStringArray))
               .ToList();
        List<string> expected = ["Germany", "Bayern", "NRW", "München", "Augsburg", "Köln", "Düsseldorf", "Laim", "Pasing"];
        list.ShouldBe(expected);
    }
    
}
