namespace Arborescence.Traversal.Adjacency.Tests;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models;
using Workbench;
using Xunit;
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
using Graph = Models.SimpleIncidenceGraph;
using EdgeEnumerator = System.ArraySegment<Int32Endpoints>.Enumerator;
#else
using Graph = Models.Compatibility.SimpleIncidenceGraph;
using EdgeEnumerator = System.Collections.Generic.IEnumerator<Int32Endpoints>;
#endif

public sealed class EnumerableBfs_Tests
{
    [Fact]
    public void EnumerateEdges_SingleSource_ReturnsCorrectEdgesInOrder()
    {
        // Arrange
        Graph.Builder builder = new(0, 31);
        using (TextReader textReader = IndexedGraphs.GetTextReader("08"))
        {
            IEnumerable<Int32Endpoints> edges = IndexedEdgeListParser.ParseEdges(textReader);
            foreach (Int32Endpoints edge in edges)
                builder.Add(edge.Tail, edge.Head);
        }

        Graph incidenceGraph = builder.ToGraph();
        var adjacencyGraph = IncidenceAdjacencyAdapter<int, Int32Endpoints, EdgeEnumerator>.Create(incidenceGraph);

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
        IEnumerable<Endpoints<int>> arrows = EnumerableBfs<int>.EnumerateEdges(adjacencyGraph, V("d"));
        var actual = arrows.ToList();

        // Assert
        Assert.Equal(expected, actual);
    }

    private static int V(string idBase32) => Base32.Parse(idBase32);
}
