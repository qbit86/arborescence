namespace Arborescence.Models;

using System.Collections.Generic;
using Xunit;

public sealed class ReadOnlyAdjacencyGraph_Tests
{
    [Fact]
    public void EnumerateOutNeighbors_AfterAdding_ShouldReturnHeadsAsNeighbors()
    {
        Dictionary<string, List<string>> neighborsByVertex = new();
        const string vertex = "μηδέν";
        string[] expectedNeighbors = { "ένα", "δύο" };
        foreach (string neighbor in expectedNeighbors)
        {
            if (neighborsByVertex.TryGetValue(vertex, out List<string>? neighbors))
                neighbors.Add(neighbor);
            else
                neighborsByVertex.Add(vertex, new(1) { neighbor });
        }

        var graph = ReadOnlyAdjacencyGraphFactory<string, List<string>, List<string>.Enumerator>.Create(
            neighborsByVertex, default(ListEnumeratorProvider<string>));

        List<string>.Enumerator actualNeighborEnumerator = graph.EnumerateOutNeighbors(vertex);
        List<string> actualNeighbors = new(expectedNeighbors.Length);
        while (actualNeighborEnumerator.MoveNext())
            actualNeighbors.Add(actualNeighborEnumerator.Current);

        Assert.Equal(expectedNeighbors, actualNeighbors);
    }
}
