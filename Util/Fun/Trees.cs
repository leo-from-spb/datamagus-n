using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Util.Fun;

/// <summary>
/// Routines for working with trees.
/// </summary>
public static class Trees
{

    /// <summary>
    /// Traverses the tree in depth-first order (the most common way to traverse a tree).
    /// First, yields the given root node.
    /// For every node, it yields the node itself and then its children;
    /// in other words, when a node is yielded, its parent, grandparent, etc. have already been yielded.
    /// </summary>
    /// <param name="root">the root to start traverse from.</param>
    /// <param name="children">a function that provides with children of a tree node.</param>
    /// <typeparam name="T">type of the tree node.</typeparam>
    /// <returns>The traversing sequence.</returns>
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
    
    /// <summary>
    /// Traverses the tree in depth-first order, but yields children before parents.
    /// For each node, it processes all children before yielding this node.
    /// So, the resulting sequence is starting with the first terminal and finishing with the root node.
    /// </summary>
    /// <param name="root">the root to start traverse from.</param>
    /// <param name="children">a function that provides with children of a tree node.</param>
    /// <typeparam name="T">type of the tree node.</typeparam>
    /// <returns>The traversing sequence.</returns>
    public static IEnumerable<T> TraversDepthChildrenFirst<T>(T root, Func<T, IEnumerable<T>> children)
        where T: notnull
    {
        foreach (var node in children(root))
        {
            foreach (var child in TraversDepthChildrenFirst(node, children))
            {
                yield return child;
            }
        }

        yield return root;
    }

    /// <summary>
    /// Traverses the tree in breadth-first order.
    /// First, yields the given root node.
    /// Then, it yields all direct children of the root.
    /// Then, it yields the second level of children, etc.
    /// For every node, it yields the node itself and then its children;
    /// in other words, when a node is yielded, its parent, grandparent, etc. have already been yielded.
    /// </summary>
    /// <param name="root">the root to start traverse from.</param>
    /// <param name="children">a function that provides with children of a tree node.</param>
    /// <typeparam name="T">type of the tree node.</typeparam>
    /// <returns>The traversing sequence.</returns>
    [SuppressMessage("ReSharper", "FunctionRecursiveOnAllPaths")]
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
