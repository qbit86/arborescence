namespace Arborescence.Models;

using System.Collections.Generic;
using Xunit;

public sealed class ListAdjacencyGraph_Tests
{
    [Fact]
    public void TryAddVertex_AfterAdding_ShouldIncreaseVertexCount()
    {
        ListAdjacencyGraph<string, Dictionary<string, List<string>>> graph = ListAdjacencyGraphFactory<string>.Create();
        Assert.True(graph.TryAddVertex("μηδέν"));
        Assert.True(graph.TryAddVertex("δύο"));
        Assert.Equal(2, graph.VertexCount);
    }

    [Fact]
    public void EnumerateOutNeighbors_AfterAdding_ShouldReturnHeadsAsNeighbors()
    {
        ListAdjacencyGraph<string, Dictionary<string, List<string>>> graph = ListAdjacencyGraphFactory<string>.Create();
        const string vertex = "μηδέν";
        string[] expectedNeighbors = { "ένα", "δύο" };
        foreach (string neighbor in expectedNeighbors)
            graph.AddEdge(vertex, neighbor);

        List<string>.Enumerator actualNeighborEnumerator = graph.EnumerateOutNeighbors(vertex);
        List<string> actualNeighbors = new(expectedNeighbors.Length);
        while (actualNeighborEnumerator.MoveNext())
            actualNeighbors.Add(actualNeighborEnumerator.Current);

        Assert.Equal(expectedNeighbors, actualNeighbors);
    }
}
