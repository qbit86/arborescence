namespace Arborescence.Models.Specialized;

using System.Collections.Generic;
using System.Linq;
using Xunit;

public sealed partial class Int32IncidenceGraphTests
{
    [Theory]
    [ClassData(typeof(GraphDefinitionCollection))]
    [ClassData(typeof(RandomGraphDefinitionCollection))]
    internal void EnumerateOutNeighbors_AllVertices_ReturnsSameSetOfVertices(GraphDefinitionParameter p)
    {
#if NET5_0_OR_GREATER
        var edges = p.Edges.ToList();
#else
        var edges = p.Edges.ToArray();
#endif
        var graph = Int32IncidenceGraph.FromEdges(edges);
        Assert.Equal(p.VertexCount, graph.VertexCount);
        Assert.Equal(p.Edges.Count, graph.EdgeCount);
        var expectedNeighborsByVertex = p.Edges.ToLookup(it => it.Tail, it => it.Head);

        for (int vertex = 0; vertex < graph.VertexCount; ++vertex)
        {
            var neighborEnumerator =
                graph.EnumerateOutNeighbors(vertex);
            List<int> actualNeighbors = new();
            while (neighborEnumerator.MoveNext())
                actualNeighbors.Add(neighborEnumerator.Current);
            var expectedNeighborsRaw = expectedNeighborsByVertex[vertex];
            if (expectedNeighborsRaw is not IList<int> expectedNeighbors)
                expectedNeighbors = expectedNeighborsRaw.ToList();
            if (expectedNeighbors.Count != actualNeighbors.Count)
            {
                Assert.Fail(
                    $"{nameof(vertex)}: {vertex}, {nameof(expectedNeighbors)}: {expectedNeighbors.Count}, {nameof(actualNeighbors)}: {actualNeighbors.Count}");
            }

            if (expectedNeighbors.Except(actualNeighbors).Any())
            {
                Assert.Fail(
                    $"{nameof(vertex)}: {vertex}, {nameof(expectedNeighbors)}.Except({nameof(actualNeighbors)}).Any()");
            }

            if (actualNeighbors.Except(expectedNeighbors).Any())
            {
                Assert.Fail(
                    $"{nameof(vertex)}: {vertex}, {nameof(actualNeighbors)}.Except({nameof(expectedNeighbors)}).Any()");
            }
        }
    }

    [Theory]
    [ClassData(typeof(GraphDefinitionCollection))]
    [ClassData(typeof(RandomGraphDefinitionCollection))]
    internal void TryGetEndpoints_AllEdges_ReturnsSameEndpoints(GraphDefinitionParameter p)
    {
        var expectedEndpointsByEdge = p.Edges;
        int edgeCount = expectedEndpointsByEdge.Count;
#if NET5_0_OR_GREATER
        var edges = expectedEndpointsByEdge.ToList();
#else
        var edges = expectedEndpointsByEdge.ToArray();
#endif
        var graph = Int32IncidenceGraph.FromEdges(edges);
        Assert.Equal(p.VertexCount, graph.VertexCount);
        Assert.Equal(edgeCount, graph.EdgeCount);

        for (int edge = 0; edge < edgeCount; ++edge)
        {
            if (!graph.TryGetTail(edge, out int actualTail))
                Assert.Fail($"{nameof(edge)}: {edge}, {nameof(graph.TryGetTail)}: false");
            if (!graph.TryGetHead(edge, out int actualHead))
                Assert.Fail($"{nameof(edge)}: {edge}, {nameof(graph.TryGetHead)}: false");
            var expectedEndpoints = expectedEndpointsByEdge[edge];
            if (expectedEndpoints.Tail != actualTail)
            {
                Assert.Fail(
                    $"{nameof(expectedEndpoints)}.Tail: {expectedEndpoints.Tail}, {nameof(actualTail)}: {actualTail}");
            }

            if (expectedEndpoints.Head != actualHead)
            {
                Assert.Fail(
                    $"{nameof(expectedEndpoints)}.Head: {expectedEndpoints.Head}, {nameof(actualHead)}: {actualHead}");
            }
        }
    }
}
