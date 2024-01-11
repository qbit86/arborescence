namespace Arborescence.Models;

using System.Collections.Generic;
using Xunit;

public sealed class ListIncidenceGraph_Tests
{
    [Fact]
    public void TryAddVertex_AfterAdding_ShouldIncreaseVertexCount()
    {
        var graph = ListIncidenceGraph<string, int>.Create();
        Assert.True(graph.TryAddVertex("μηδέν"));
        Assert.True(graph.TryAddVertex("δύο"));
        Assert.Equal(2, graph.MinVertexCount);
    }

    [Fact]
    public void EnumerateOutNeighbors_AfterAdding_ShouldReturnHeadsAsNeighbors()
    {
        var graph = ListIncidenceGraph<string, int>.Create();
        const string vertex = "μηδέν";
        string[] expectedNeighbors = { "ένα", "δύο" };
        foreach (string neighbor in expectedNeighbors)
            Assert.True(graph.TryAddEdge(graph.EdgeCount, vertex, neighbor));

        // ReSharper disable once SuggestVarOrType_Elsewhere
        var actualNeighborEnumerator = graph.EnumerateOutNeighbors(vertex);
        List<string> actualNeighbors = new(expectedNeighbors.Length);
        while (actualNeighborEnumerator.MoveNext())
            actualNeighbors.Add(actualNeighborEnumerator.Current);

        Assert.Equal(expectedNeighbors, actualNeighbors);
    }
}
