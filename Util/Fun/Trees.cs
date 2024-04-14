using System;
using System.Collections.Generic;

namespace Util.Fun;

public static class Trees
{

    public static IEnumerable<T> TraversDepthFirst<T>(T root, Func<T, IEnumerable<T>> children)
        where T: notnull
    {
        yield return root;

        foreach (var node in children(root))
        {
            foreach (var child in TraversDepthFirst(node, children))
            {
                yield return child;
            }
        }
    }
    
    
    public static IEnumerable<T> TraverseBreadthFirst<T>(T root, Func<T, IEnumerable<T>> children)
        where T: notnull
    {
        yield return root;

        T last = root;
        foreach (var node in TraverseBreadthFirst(root, children))
        {
            foreach (var child in children(node))
            {
                yield return child;
                last = child;
            }
            if (last.Equals(node)) yield break;
        }
    }
    
}
