namespace Arborescence.Models.Specialized;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Workbench;
using Xunit;

public sealed partial class Int32AdjacencyGraphTests
{
    [Fact]
    internal void EnumerateOutNeighbors_ExistingVertex_ReturnsKnownVertices()
    {
        // Arrange
        using TextReader textReader = IndexedGraphs.GetTextReader("09");
        IEnumerable<Endpoints<int>> rawEdges = Base32EdgeListParser.ParseEdges(textReader);
#if NET5_0_OR_GREATER
        var edges = rawEdges.ToList();
#else
        Endpoints<int>[] edges = rawEdges.ToArray();
#endif
        int vertex = Base32.Parse("p");
        var expectedNeighbors = new HashSet<int>
            { Base32.Parse("f"), Base32.Parse("m"), Base32.Parse("q"), Base32.Parse("r") };

        // Act
        var graph = Int32AdjacencyGraph.FromEdges(edges);
        ArraySegment<int>.Enumerator neighborEnumerator = graph.EnumerateOutNeighbors(vertex);
        HashSet<int> actualNeighbors = new(4);
        while (neighborEnumerator.MoveNext())
            actualNeighbors.Add(neighborEnumerator.Current);

        // Assert
        Assert.Equal(expectedNeighbors, actualNeighbors);
    }

    [Fact]
    internal void EnumerateOutEdges_ExistingVertex_ReturnsKnownEdges()
    {
        // Arrange
        using TextReader textReader = IndexedGraphs.GetTextReader("09");
        IEnumerable<Endpoints<int>> rawEdges = Base32EdgeListParser.ParseEdges(textReader);
#if NET5_0_OR_GREATER
        var edges = rawEdges.ToList();
#else
        Endpoints<int>[] edges = rawEdges.ToArray();
#endif
        int vertex = Base32.Parse("p");
        var expectedEdges = new HashSet<Endpoints<int>>
        {
            Create("p", "f"),
            Create("p", "q"),
            Create("p", "m"),
            Create("p", "r"),
        };

        static Endpoints<int> Create(ReadOnlySpan<char> tail, ReadOnlySpan<char> head) =>
            Endpoints.Create(Base32.Parse(tail), Base32.Parse(head));

        // Act
        var graph = Int32AdjacencyGraph.FromEdges(edges);
        IncidenceEnumerator<int, ArraySegment<int>.Enumerator> edgeEnumerator = graph.EnumerateOutEdges(vertex);
        HashSet<Endpoints<int>> actualEdges = new(4);
        while (edgeEnumerator.MoveNext())
            actualEdges.Add(edgeEnumerator.Current);

        // Assert
        Assert.Equal(expectedEdges, actualEdges);
    }
}
