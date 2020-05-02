namespace Ubiquitous
{
    using System.Diagnostics;
    using Models;
    using Traversal;
    using Xunit;
    using Xunit.Abstractions;
    using EdgeEnumerator = ArraySegmentEnumerator<int>;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    public sealed class IterativeDfsEnumerateVerticesTest
    {
        public IterativeDfsEnumerateVerticesTest(ITestOutputHelper output)
        {
            var graphPolicy = default(IndexedAdjacencyListGraphPolicy);
            var colorMapPolicy = default(IndexedColorMapPolicy);

            InstantDfs = InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[]>.Create(
                graphPolicy, colorMapPolicy);

            IterativeDfs = IterativeDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[]>.Create(
                graphPolicy, colorMapPolicy);

            Output = output;
        }

        private InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            InstantDfs { get; }

        private IterativeDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            IterativeDfs { get; }

        private ITestOutputHelper Output { get; }

#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Theory(Skip = "Not implemented yet")]
        [InlineData(1.0)]
        [InlineData(1.414)]
        [InlineData(1.618)]
        [InlineData(2.0)]
        public void Iterative_vertices_single_component(double densityPower)
        {
            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(100, densityPower);
            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
