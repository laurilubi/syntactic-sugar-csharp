using System.Collections.Generic;
using System.Linq;
using Syntactic.Sugar.Core.Tree;
using Xunit;

namespace Core.Tests.Tree;

public class TreeToolsTests
{
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
    public void TraversesChildren()
    {
        var descendants = TreeTools.GetDescendants(ChildrenModel, item => item.Children);

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

    [Fact]
    public void IncludesRootWhenRequested()
    {
        var descendants = TreeTools.GetDescendants(RootModel, item => item.Children, includeRoot: true);

        Assert.Equal(6, descendants.Count);
        Assert.Equal("root", descendants[0].Id);
        Assert.Equal("A", descendants[1].Id);
    }

    [Fact]
    public void SkipsOnLoop()
    {
        var root = RootModel;
        root.Children[0].Children[0].Children.Add(root.Children[1]); // create loop by adding B to child of A

        var descendants = TreeTools.GetDescendants(root, item => item.Children, loopBehavior: TreeLoopBehavior.Skip);

        Assert.Equal(5, descendants.Count);
        Assert.Equal(1, descendants.Count(item => item == root.Children[1]));
    }

    [Fact]
    public void ThrowsOnLoop()
    {
        var root = RootModel;
        root.Children[0].Children[0].Children.Add(root.Children[1]); // create loop by adding B to child of A

        Assert.Throws<TreeLoopException>(() =>
            TreeTools.GetDescendants(root, item => item.Children, loopBehavior: TreeLoopBehavior.Throw));
    }

    private Item RootModel => new()
    {
        Id = "root",
        Children = ChildrenModel
    };

    private Tree TreeModel => new()
    {
        RootChildren = ChildrenModel
    };

    private List<Item> ChildrenModel => new()
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