namespace Arborescence;

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Models;
using Graph = Models.SimpleIncidenceGraph;
using EdgeEnumerator = System.ArraySegment<Int32Endpoints>.Enumerator;

public sealed class UndirectedSimpleIncidenceGraphFromMutableTest
{
    private static IEqualityComparer<HashSet<Int32Endpoints>> HashSetEqualityComparer { get; } =
        HashSet<Int32Endpoints>.CreateSetComparer();

    private static bool TryGetEndpoints(Graph graph, Int32Endpoints edge, out Int32Endpoints endpoints)
    {
        bool hasTail = graph.TryGetTail(edge, out int tail);
        bool hasHead = graph.TryGetHead(edge, out int head);
        endpoints = new(tail, head);
        return hasTail && hasHead;
    }

    [Theory]
    [ClassData(typeof(GraphDefinitionCollection))]
    internal void Graph_SizeShouldMatch(GraphDefinitionParameter p)
    {
        // Arrange
        using MutableUndirectedSimpleIncidenceGraph builder = new(p.VertexCount);
        foreach (Int32Endpoints endpoints in p.Edges)
            builder.Add(endpoints.Tail, endpoints.Head);

        // Act
        Graph graph = builder.ToGraph();

        // Assert
        Assert.Equal(p.VertexCount, graph.VertexCount);
        Assert.Equal(p.Edges.Count, graph.EdgeCount);
    }

    [Theory]
    [ClassData(typeof(GraphDefinitionCollection))]
    internal void Graph_ShouldContainSameSetOfEdges(GraphDefinitionParameter p)
    {
        // Arrange
        using MutableUndirectedSimpleIncidenceGraph builder = new(p.VertexCount);
        foreach (Int32Endpoints endpoints in p.Edges)
            builder.Add(endpoints.Tail, endpoints.Head);

        Graph graph = builder.ToGraph();
        var expectedEdgeSet = p.Edges.ToHashSet();
        foreach (Int32Endpoints edge in p.Edges)
        {
            if (edge.Tail == edge.Head)
                continue;
            Int32Endpoints invertedEdge = new(edge.Head, edge.Tail);
            expectedEdgeSet.Add(invertedEdge);
        }

        // Act
        HashSet<Int32Endpoints> actualEdgeSet = new();
        for (int vertex = 0; vertex < graph.VertexCount; ++vertex)
        {
            EdgeEnumerator outEdges = graph.EnumerateOutEdges(vertex);
            while (outEdges.MoveNext())
            {
                Int32Endpoints edge = outEdges.Current;
                bool hasEndpoints = TryGetEndpoints(graph, edge, out Int32Endpoints endpoints);
                if (!hasEndpoints)
                    Assert.True(hasEndpoints);

                actualEdgeSet.Add(endpoints);
            }
        }

        // Assert
        Assert.Equal(expectedEdgeSet, actualEdgeSet, HashSetEqualityComparer);
    }

    [Theory]
    [ClassData(typeof(GraphDefinitionCollection))]
    internal void Graph_OutEdgesShouldHaveSameTail(GraphDefinitionParameter p)
    {
        // Arrange
        using MutableUndirectedSimpleIncidenceGraph builder = new(p.VertexCount);
        foreach (Int32Endpoints endpoints in p.Edges)
            builder.Add(endpoints.Tail, endpoints.Head);

        Graph graph = builder.ToGraph();

        // Act
        for (int vertex = 0; vertex < graph.VertexCount; ++vertex)
        {
            EdgeEnumerator outEdges = graph.EnumerateOutEdges(vertex);
            while (outEdges.MoveNext())
            {
                Int32Endpoints edge = outEdges.Current;
                bool hasTail = graph.TryGetTail(edge, out int tail);
                if (!hasTail)
                    Assert.True(hasTail);

                // Assert
                Assert.Equal(vertex, tail);
            }
        }
    }
}
