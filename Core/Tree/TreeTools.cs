using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Syntactic.Sugar.Core.Enum;

namespace Syntactic.Sugar.Core.Tree;

public static class TreeTools
{
    public static List<TItem> GetDescendants<TItem>(
        TItem root,
        Func<TItem, IEnumerable<TItem>> childrenSelector,
        bool includeRoot = false,
        TreeLoopBehavior loopBehavior = TreeLoopBehavior.Throw)
    {
        AssertTools.Assert(EnumTools.IsValidValue(loopBehavior),
            () => new ArgumentException(nameof(loopBehavior)));

        var descendants = new List<TItem>();
        if (includeRoot)
        {
            descendants.Add(root);
        }

        AddDescendants(descendants, root, childrenSelector, loopBehavior);

        return descendants;
    }

    public static List<TItem> GetDescendants<TItem>(
        IEnumerable<TItem> roots,
        Func<TItem, IEnumerable<TItem>> childrenSelector,
        TreeLoopBehavior loopBehavior = TreeLoopBehavior.Throw)
    {
        var tree = roots.ToList();
        return GetDescendants(
            tree,
            tree => tree,
            childrenSelector,
            loopBehavior);
    }

    public static List<TItem> GetDescendants<TTree, TItem>(
        TTree tree,
        Func<TTree, IEnumerable<TItem>> rootChildrenSelector,
        Func<TItem, IEnumerable<TItem>> childrenSelector,
        TreeLoopBehavior loopBehavior = TreeLoopBehavior.Throw)
    {
        AssertTools.Assert(EnumTools.IsValidValue(loopBehavior),
            () => new ArgumentException(nameof(loopBehavior)));

        var descendants = new List<TItem>();

        var children = rootChildrenSelector(tree);
        foreach (var child in children)
        {
            descendants.Add(child);
            AddDescendants(descendants, child, childrenSelector, loopBehavior);
        }

        return descendants;
    }

    private static void AddDescendants<TItem>(
        List<TItem> descendants,
        TItem root,
        Func<TItem, IEnumerable<TItem>> childrenSelector,
        TreeLoopBehavior loopBehavior)
    {
        var children = childrenSelector(root);

        foreach (var child in children)
        {
            if (loopBehavior != TreeLoopBehavior.NoCheck)
            {
                var childAlreadyAdded = descendants.Contains(child);
                if (childAlreadyAdded)
                {
                    if (loopBehavior == TreeLoopBehavior.Throw)
                        throw new TreeLoopException($"Tree loop by item {child}");
                    else
                        continue;
                }
            }

            descendants.Add(child);

            AddDescendants(descendants, child, childrenSelector, loopBehavior);
        }
    }
}