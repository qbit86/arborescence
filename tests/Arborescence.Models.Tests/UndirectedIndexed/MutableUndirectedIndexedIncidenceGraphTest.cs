namespace Arborescence
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Graph = Models.MutableUndirectedIndexedIncidenceGraph;
    using EdgeEnumerator = ArrayPrefixEnumerator<int>;

    public sealed class MutableUndirectedIndexedIncidenceGraphTest
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
            using var graph = new Graph(p.VertexCount, p.Edges.Count);
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
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
