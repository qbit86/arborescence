namespace Arborescence.Models;

using System.Collections.Generic;
using Xunit;

public sealed class ListIncidenceGraph_Tests
{
    [Fact]
    public void TryAddVertex_AfterAdding_ShouldIncreaseVertexCount()
    {
        ListIncidenceGraph<string, int, Dictionary<int, string>, Dictionary<string, List<int>>> graph =
            ListIncidenceGraphFactory<string, int>.Create();
        Assert.True(graph.TryAddVertex("μηδέν"));
        Assert.True(graph.TryAddVertex("δύο"));
        Assert.Equal(2, graph.VertexCount);
    }

    [Fact]
    public void EnumerateOutNeighbors_AfterAdding_ShouldReturnHeadsAsNeighbors()
    {
        ListIncidenceGraph<string, int, Dictionary<int, string>, Dictionary<string, List<int>>> graph =
            ListIncidenceGraphFactory<string, int>.Create();
        const string vertex = "μηδέν";
        string[] expectedNeighbors = { "ένα", "δύο" };
        foreach (string neighbor in expectedNeighbors)
            Assert.True(graph.TryAddEdge(graph.EdgeCount, vertex, neighbor));

        var actualNeighborEnumerator = graph.EnumerateOutNeighbors(vertex);
        List<string> actualNeighbors = new(expectedNeighbors.Length);
        while (actualNeighborEnumerator.MoveNext())
            actualNeighbors.Add(actualNeighborEnumerator.Current);

        Assert.Equal(expectedNeighbors, actualNeighbors);
    }
}