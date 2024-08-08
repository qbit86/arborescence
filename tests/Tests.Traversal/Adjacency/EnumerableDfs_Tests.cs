namespace Arborescence.Traversal.Adjacency;

using System;
using System.Collections.Generic;
using System.Linq;
using Models.Specialized;
using Workbench;
using Xunit;

public sealed class EnumerableDfs_Tests
{
    [Fact]
    public void EnumerateEdges_MultipleSource_ReturnsCorrectEdgesInOrder()
    {
        // Arrange
        using var textReader = IndexedGraphs.GetTextReader("08");
        var edges = Base32EdgeListParser.ParseEdges(textReader).ToArray();
        textReader.Close();

        var adjacencyGraph = Int32AdjacencyGraph.FromEdges(edges);

        List<(string Tail, string Head)> expectedBase32 = new(12)
        {
            ("a", "f"),
            ("f", "j"),
            ("j", "n"),
            ("j", "r"),
            ("r", "e"),
            ("e", "i"),
            ("r", "q"),
            ("f", "p"),
            ("p", "m"),

            ("d", "g"),
            ("g", "k"),

            ("b", "h"),
            ("h", "l"),
            ("l", "o")
        };
        var expected = expectedBase32
            .Select(it => Endpoints.Create(V(it.Tail), V(it.Head))).ToList();

        // Act
        List<int> sources = new(3) { V("a"), V("d"), V("b") };
        var arrows =
            EnumerableDfs<int, ArraySegment<int>.Enumerator>.EnumerateEdges(adjacencyGraph, sources);
        var actual = arrows.ToList();

        // Assert
        Assert.Equal(expected, actual);
    }

    private static int V(string idBase32) => Base32.Parse(idBase32);
}
