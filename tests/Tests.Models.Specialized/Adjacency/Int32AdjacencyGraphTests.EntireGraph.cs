namespace Arborescence.Models.Specialized;

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public sealed partial class Int32AdjacencyGraphTests
{
    [Theory]
    [ClassData(typeof(GraphDefinitionCollection))]
    internal void EnumerateOutNeighbors_AllVertices_ReturnsSameSetOfVertices(GraphDefinitionParameter p)
    {
#if NET5_0_OR_GREATER
        var edges = p.Edges.ToList();
#else
        Endpoints<int>[] edges = p.Edges.ToArray();
#endif
        Int32AdjacencyGraph graph = Int32AdjacencyGraphFactory.FromEdges(edges);
        Assert.Equal(p.VertexCount, graph.VertexCount);
        Assert.Equal(p.Edges.Count, graph.EdgeCount);
        ILookup<int, int> expectedNeighborsByVertex = p.Edges.ToLookup(it => it.Tail, it => it.Head);

        for (int vertex = 0; vertex < graph.VertexCount; ++vertex)
        {
            ArraySegment<int>.Enumerator neighborEnumerator = graph.EnumerateOutNeighbors(vertex);
            List<int> actualNeighbors = new();
            while (neighborEnumerator.MoveNext())
                actualNeighbors.Add(neighborEnumerator.Current);
            IEnumerable<int> expectedNeighborsRaw = expectedNeighborsByVertex[vertex];
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
}
