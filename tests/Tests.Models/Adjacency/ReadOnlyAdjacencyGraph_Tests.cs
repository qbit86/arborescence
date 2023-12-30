namespace Arborescence.Models;

using System.Collections.Generic;
using Xunit;

public sealed class ReadOnlyAdjacencyGraph_Tests
{
    [Fact]
    public void EnumerateOutNeighbors_AfterAdding_ShouldReturnHeadsAsNeighbors()
    {
        const string vertex = "μηδέν";
        string[] expectedNeighbors = { "ένα", "δύο" };
        Dictionary<string, List<string>> neighborsByVertex = new();
        foreach (string neighbor in expectedNeighbors)
        {
            if (neighborsByVertex.TryGetValue(vertex, out List<string>? neighbors))
                neighbors.Add(neighbor);
            else
                neighborsByVertex.Add(vertex, [neighbor]);
        }

        var graph = ReadOnlyAdjacencyGraphFactory<string>.FromLists(neighborsByVertex);

        List<string>.Enumerator actualNeighborEnumerator = graph.EnumerateOutNeighbors(vertex);
        List<string> actualNeighbors = new(expectedNeighbors.Length);
        while (actualNeighborEnumerator.MoveNext())
            actualNeighbors.Add(actualNeighborEnumerator.Current);

        Assert.Equal(expectedNeighbors, actualNeighbors);
    }
}
