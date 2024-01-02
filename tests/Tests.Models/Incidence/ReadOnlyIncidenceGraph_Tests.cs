namespace Arborescence.Models;

using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

public sealed class ReadOnlyIncidenceGraph_Tests
{
    [Fact]
    public void EnumerateOutNeighbors_AfterAdding_ShouldReturnHeadsAsNeighbors()
    {
        const string vertex = "μηδέν";
        string[] expectedNeighbors = { "ένα", "δύο" };
        List<string> tailByEdge = new();
        List<string> headByEdge = new();
        Dictionary<string, List<int>> outEdgesByVertex = new();
        foreach (string neighbor in expectedNeighbors)
        {
            Debug.Assert(tailByEdge.Count == headByEdge.Count);
            int edge = tailByEdge.Count;
            tailByEdge.Add(vertex);
            headByEdge.Add(neighbor);
            if (outEdgesByVertex.TryGetValue(vertex, out List<int>? outEdges))
                outEdges.Add(edge);
            else
                outEdgesByVertex.Add(vertex, [edge]);
        }

        var graph = ReadOnlyIncidenceGraph<string, int>.FromLists(
            Int32ReadOnlyDictionary<string>.Create(tailByEdge),
            Int32ReadOnlyDictionary<string>.Create(headByEdge),
            outEdgesByVertex);

        var actualNeighborEnumerator = graph.EnumerateOutNeighbors(vertex);
        List<string> actualNeighbors = new(expectedNeighbors.Length);
        while (actualNeighborEnumerator.MoveNext())
            actualNeighbors.Add(actualNeighborEnumerator.Current);

        Assert.Equal(expectedNeighbors, actualNeighbors);
    }
}
