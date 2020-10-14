namespace Arborescence
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;
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
            endpoints = new Endpoints(tail, head);
            return hasTail && hasHead;
        }

#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Theory]
        [ClassData(typeof(GraphDefinitionCollection))]
        internal void Graph_SizeShouldMatch(GraphDefinitionParameter p)
        {
            // Arrange
            var builder = new Graph.Builder(p.VertexCount, p.Edges.Count);
            foreach (Endpoints endpoints in p.Edges)
            {
                bool wasAdded = builder.TryAdd(endpoints.Tail, endpoints.Head);
                if (!wasAdded)
                    Assert.True(wasAdded);
            }

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
            var builder = new Graph.Builder(p.VertexCount, p.Edges.Count);
            foreach (Endpoints endpoints in p.Edges)
            {
                bool wasAdded = builder.TryAdd(endpoints.Tail, endpoints.Head);
                if (!wasAdded)
                    Assert.True(wasAdded);
            }

            Graph graph = builder.ToGraph();
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

        [Theory]
        [ClassData(typeof(GraphDefinitionCollection))]
        internal void Graph_OutEdgesShouldHaveSameTail(GraphDefinitionParameter p)
        {
            // Arrange
            var builder = new Graph.Builder(p.VertexCount, p.Edges.Count);
            foreach (Endpoints endpoints in p.Edges)
            {
                bool wasAdded = builder.TryAdd(endpoints.Tail, endpoints.Head);
                if (!wasAdded)
                    Assert.True(wasAdded);
            }

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
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
