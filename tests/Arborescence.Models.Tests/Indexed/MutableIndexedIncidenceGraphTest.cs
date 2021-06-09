namespace Arborescence
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using EdgeEnumerator = System.ArraySegment<int>.Enumerator;
    using Graph = Models.MutableIndexedIncidenceGraph;

    public sealed class MutableIndexedIncidenceGraphTest
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
            using Graph graph = new(p.VertexCount, p.Edges.Count);
            foreach (Endpoints endpoints in p.Edges)
                graph.Add(endpoints.Tail, endpoints.Head);

            // Assert
            Assert.Equal(p.VertexCount, graph.VertexCount);
            Assert.Equal(p.Edges.Count, graph.EdgeCount);
        }

        [Theory]
        [ClassData(typeof(GraphDefinitionCollection))]
        internal void Graph_ShouldContainSameSetOfEdges(GraphDefinitionParameter p)
        {
            // Arrange
            using Graph graph = new(p.VertexCount, p.Edges.Count);
            foreach (Endpoints endpoints in p.Edges)
                graph.Add(endpoints.Tail, endpoints.Head);

            HashSet<Endpoints> expectedEdgeSet = p.Edges.ToHashSet();

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
            using Graph graph = new(p.VertexCount, p.Edges.Count);
            foreach (Endpoints endpoints in p.Edges)
                graph.Add(endpoints.Tail, endpoints.Head);

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
