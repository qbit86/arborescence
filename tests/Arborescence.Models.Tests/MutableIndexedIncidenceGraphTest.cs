﻿namespace Arborescence
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Graph = Models.MutableIndexedIncidenceGraph;
    using EdgeEnumerator = ArrayPrefixEnumerator<int>;

    public sealed class MutableIndexedIncidenceGraphTest
    {
        private static IEqualityComparer<HashSet<Endpoints>> HashSetEqualityComparer { get; } =
            HashSet<Endpoints>.CreateSetComparer();

        private static bool TryGetEndpoints(Graph graph, int edge, out Endpoints endpoints)
        {
            if (!graph.TryGetTail(edge, out int tail))
            {
                endpoints = default;
                return false;
            }

            if (!graph.TryGetHead(edge, out int head))
            {
                endpoints = default;
                return false;
            }

            endpoints = new Endpoints(tail, head);
            return true;
        }

#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Theory]
        [ClassData(typeof(GraphDefinitionCollection))]
        internal void Graph_SizeShouldMatch(GraphDefinitionParameter p)
        {
            // Arrange
            var graph = new Graph(p.VertexCount, p.Edges.Count);
            foreach (Endpoints endpoints in p.Edges)
            {
                bool wasAdded = graph.TryAdd(endpoints.Tail, endpoints.Head, out _);
                if (!wasAdded)
                    Assert.True(wasAdded);
            }

            // Assert
            Assert.Equal(p.VertexCount, graph.VertexCount);
            Assert.Equal(p.Edges.Count, graph.EdgeCount);
        }

        [Theory]
        [ClassData(typeof(GraphDefinitionCollection))]
        internal void Graph_ShouldContainSameSetOfEdges(GraphDefinitionParameter p)
        {
            // Arrange
            var graph = new Graph(p.VertexCount, p.Edges.Count);
            foreach (Endpoints endpoints in p.Edges)
            {
                bool wasAdded = graph.TryAdd(endpoints.Tail, endpoints.Head, out _);
                if (!wasAdded)
                    Assert.True(wasAdded);
            }

            HashSet<Endpoints> expectedEdgeSet = p.Edges.ToHashSet();

            // Act
            var actualEdgeSet = new HashSet<Endpoints>();
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
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
