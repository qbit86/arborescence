namespace Arborescence.Adjacency;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models.Specialized;
using Workbench;
using Xunit;

public sealed class Int32AdjacencyGraphTests
{
    [Fact]
    internal void EnumerateOutNeighbors_ExistingVertex_ReturnsKnownVertices()
    {
        // Arrange
        using TextReader textReader = IndexedGraphs.GetTextReader("09");
        IEnumerable<Int32Endpoints> rawEdges = IndexedEdgeListParser.ParseEdges(textReader);
#if NET5_0_OR_GREATER
        var edges = rawEdges.Select(Transform).ToList();
#else
        Endpoints<int>[] edges = rawEdges.Select(Transform).ToArray();
#endif
        int vertex = Base32.Parse("p");
        var expectedNeighbors = new HashSet<int>
            { Base32.Parse("f"), Base32.Parse("m"), Base32.Parse("q"), Base32.Parse("r") };

        static Endpoints<int> Transform(Int32Endpoints endpoints) => new(endpoints.Tail, endpoints.Head);

        // Act
        Int32FrozenAdjacencyGraph graph = Int32FrozenAdjacencyGraphFactory.FromEdges(edges);
        ArraySegment<int>.Enumerator neighborEnumerators = graph.EnumerateOutNeighbors(vertex);
        HashSet<int> actualNeighbors = new(4);
        while (neighborEnumerators.MoveNext())
            actualNeighbors.Add(neighborEnumerators.Current);

        // Assert
        Assert.Equal(expectedNeighbors, actualNeighbors);
    }
}
