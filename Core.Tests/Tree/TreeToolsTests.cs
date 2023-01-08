using System.Collections.Generic;
using Syntactic.Sugar.Core.Tree;
using Xunit;

namespace Core.Tests.Tree;

public class TreeToolsTests
{
    private static readonly List<Item> Children = new()
    {
        new()
        {
            Id = "A",
            Children = new()
            {
                new() { Id = "A-1" },
                new() { Id = "A-2" }
            }
        },
        new()
        {
            Id = "B",
            Children = new()
            {
                new() { Id = "B-1" }
            }
        }
    };

    private static readonly Item RootModel = new()
    {
        Id = "root",
        Children = Children
    };

    private static readonly Tree TreeModel = new()
    {
        RootChildren = Children
    };

    [Fact]
    public void TraversesRoot()
    {
        var descendants = TreeTools.GetDescendants(RootModel, item => item.Children);

        Assert.Equal(5, descendants.Count);
        Assert.Equal("A", descendants[0].Id);
        Assert.Equal("A-1", descendants[1].Id);
        Assert.Equal("A-2", descendants[2].Id);
        Assert.Equal("B", descendants[3].Id);
        Assert.Equal("B-1", descendants[4].Id);
    }

    [Fact]
    public void TraversesRoots()
    {
        var descendants = TreeTools.GetDescendants(Children, item => item.Children);

        Assert.Equal(5, descendants.Count);
        Assert.Equal("A", descendants[0].Id);
        Assert.Equal("A-1", descendants[1].Id);
        Assert.Equal("A-2", descendants[2].Id);
        Assert.Equal("B", descendants[3].Id);
        Assert.Equal("B-1", descendants[4].Id);
    }

    [Fact]
    public void TraversesTree()
    {
        var descendants = TreeTools.GetDescendants(TreeModel, tree => tree.RootChildren, item => item.Children);

        Assert.Equal(5, descendants.Count);
        Assert.Equal("A", descendants[0].Id);
        Assert.Equal("A-1", descendants[1].Id);
        Assert.Equal("A-2", descendants[2].Id);
        Assert.Equal("B", descendants[3].Id);
        Assert.Equal("B-1", descendants[4].Id);
    }

    public class Tree
    {
        public List<Item> RootChildren { get; set; } = new();
    }

    public class Item
    {
        public string Id { get; set; }
        public List<Item> Children { get; set; } = new();
        public override string ToString() => Id;
    }
}