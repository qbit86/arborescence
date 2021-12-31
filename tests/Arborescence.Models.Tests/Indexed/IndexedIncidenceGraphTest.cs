﻿namespace Arborescence
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Graph = Models.IndexedIncidenceGraph;
    using EdgeEnumerator = System.ArraySegment<int>.Enumerator;

    public sealed class IndexedIncidenceGraphTest
    {
        private static IEqualityComparer<HashSet<Endpoints>> HashSetEqualityComparer { get; } =
            HashSet<Endpoints>.CreateSetComparer();

        private static bool TryGetEndpoints(Graph graph, int edge, out Endpoints endpoints)
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
            Graph.Builder builder = new(p.VertexCount, p.Edges.Count);
            foreach (Endpoints endpoints in p.Edges)
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
            Graph.Builder builder = new(p.VertexCount, p.Edges.Count);
            foreach (Endpoints endpoints in p.Edges)
                builder.Add(endpoints.Tail, endpoints.Head);

            Graph graph = builder.ToGraph();
            var expectedEdgeSet = p.Edges.ToHashSet();

            // Act
            HashSet<Endpoints> actualEdgeSet = new();
            for (int vertex = 0; vertex < graph.VertexCount; ++vertex)
            {
                EdgeEnumerator outEdges = graph.EnumerateOutEdges(vertex);
                while (outEdges.MoveNext())
                {
                    int edge = outEdges.Current;
                    bool hasEndpoints = TryGetEndpoints(graph, edge, out Endpoints endpoints);
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
            Graph.Builder builder = new(p.VertexCount, p.Edges.Count);
            foreach (Endpoints endpoints in p.Edges)
                builder.Add(endpoints.Tail, endpoints.Head);

            Graph graph = builder.ToGraph();

            // Act
            for (int vertex = 0; vertex < graph.VertexCount; ++vertex)
            {
                EdgeEnumerator outEdges = graph.EnumerateOutEdges(vertex);
                while (outEdges.MoveNext())
                {
                    int edge = outEdges.Current;
                    bool hasTail = graph.TryGetTail(edge, out int tail);
                    if (!hasTail)
                        Assert.True(hasTail);

                    // Assert
                    Assert.Equal(vertex, tail);
                }
            }
        }
    }
}
