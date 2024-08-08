namespace Arborescence.Models.Specialized;

using System.Collections.Generic;
using System.Linq;
using Workbench;
using Xunit;

public sealed partial class Int32IncidenceGraphTests
{
    [Fact]
    internal void EnumerateOutNeighbors_ExistingVertex_ReturnsKnownVertices()
    {
        // Arrange
        using var textReader = IndexedGraphs.GetTextReader("09");
        var rawEdges = Base32EdgeListParser.ParseEdges(textReader);
#if NET5_0_OR_GREATER
        var edges = rawEdges.ToList();
#else
        var edges = rawEdges.ToArray();
#endif
        int vertex = Base32.Parse("p");
        var expectedNeighbors = new HashSet<int>
            { Base32.Parse("f"), Base32.Parse("m"), Base32.Parse("q"), Base32.Parse("r") };

        // Act
        var graph = Int32IncidenceGraph.FromEdges(edges);
        var neighborEnumerator = graph.EnumerateOutNeighbors(vertex);
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
        using var textReader = IndexedGraphs.GetTextReader("09");
        var rawEdges = Base32EdgeListParser.ParseEdges(textReader);
#if NET5_0_OR_GREATER
        var edges = rawEdges.ToList();
#else
        var edges = rawEdges.ToArray();
#endif
        int vertex = Base32.Parse("p");
        var expectedEdges = new HashSet<int> { 6, 8, 23, 25 };

        // Act
        var graph = Int32IncidenceGraph.FromEdges(edges);
        var edgeEnumerator = graph.EnumerateOutEdges(vertex);
        HashSet<int> actualEdges = new(4);
        while (edgeEnumerator.MoveNext())
            actualEdges.Add(edgeEnumerator.Current);

        // Assert
        Assert.Equal(expectedEdges, actualEdges);
    }
}
