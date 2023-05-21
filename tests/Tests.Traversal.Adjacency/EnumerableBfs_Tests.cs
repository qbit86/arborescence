namespace Arborescence.Traversal.Adjacency;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models.Specialized;
using Workbench;
using Xunit;

public sealed class EnumerableBfs_Tests
{
    private static Endpoints<int> Transform(Int32Endpoints endpoints) => new(endpoints.Tail, endpoints.Head);

    [Fact]
    public void EnumerateEdges_SingleSource_ReturnsCorrectEdgesInOrder()
    {
        // Arrange
        using TextReader textReader = IndexedGraphs.GetTextReader("08");
        Endpoints<int>[] edges = IndexedEdgeListParser.ParseEdges(textReader).Select(Transform).ToArray();
        textReader.Dispose();

        Int32AdjacencyGraph adjacencyGraph = Int32AdjacencyGraphFactory.FromEdges(edges);

        List<(string Tail, string Head)> expectedBase32 = new(12)
        {
            ("d", "g"),
            ("d", "k"),
            ("g", "a"),
            ("a", "f"),
            ("a", "j"),
            ("a", "n"),
            ("f", "p"),
            ("j", "r"),
            ("p", "m"),
            ("p", "q"),
            ("r", "e"),
            ("m", "i")
        };
        var expected = expectedBase32
            .Select(it => Endpoints.Create(V(it.Tail), V(it.Head))).ToList();

        // Act
        IEnumerable<Endpoints<int>> arrows =
            EnumerableBfs<int, System.ArraySegment<int>.Enumerator>.EnumerateEdges(adjacencyGraph, V("d"));
        var actual = arrows.ToList();

        // Assert
        Assert.Equal(expected, actual);
    }

    private static int V(string idBase32) => Base32.Parse(idBase32);
}
