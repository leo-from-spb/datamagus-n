using System.Collections.Generic;
using System.Linq;
using Util.Extensions;
using static Util.Fun.Trees;

namespace Util.Fun;


[TestFixture]
public class TreesTest
{
    /// <summary>
    /// <pre>
    /// Germany
    ///     Bayern
    ///         München
    ///             Lime
    ///             Pasing
    ///             Schwabing
    ///         Augsburg
    ///     NRW
    ///         Köln
    ///         Düsseldorf
    ///     Berlin
    /// </pre>
    /// </summary>
    private SortedList<string, string[]> SimpleTree
        = new()
          {
              { "Germany", ["Bayern", "NRW", "Berlin"] },
              { "Bayern", ["München", "Augsburg"] },
              { "München", ["Laim", "Pasing", "Schwabing"] },
              { "NRW", ["Köln", "Düsseldorf"] },
          };

    private readonly string[] EmptyStringArray = [];
    
    [Test]
    public void TraversDepthFirst_basic()
    {
        var list = 
            TraversDepthFirst("Germany", s => SimpleTree.Get(s, EmptyStringArray))
               .ToList();
        List<string> expected = ["Germany", "Bayern", "München", "Laim", "Pasing", "Schwabing", "Augsburg", "NRW", "Köln", "Düsseldorf", "Berlin"];
        list.ShouldBe(expected);
    }
    
    [Test]
    public void TraversDepthChildrenFirst_basic()
    {
        var list =
            TraversDepthChildrenFirst("Germany", s => SimpleTree.Get(s, EmptyStringArray))
               .ToList();
        List<string> expected = ["Laim", "Pasing", "Schwabing", "München", "Augsburg", "Bayern", "Köln", "Düsseldorf", "NRW", "Berlin", "Germany"];
        list.ShouldBe(expected);
    }

    [Test]
    public void TraversBreadthFirst_basic()
    {
        var list = 
            TraverseBreadthFirst("Germany", s => SimpleTree.Get(s, EmptyStringArray))
               .ToList();
        List<string> expected = ["Germany", "Bayern", "NRW", "Berlin", "München", "Augsburg", "Köln", "Düsseldorf", "Laim", "Pasing", "Schwabing"];
        list.ShouldBe(expected);
    }
    
}
