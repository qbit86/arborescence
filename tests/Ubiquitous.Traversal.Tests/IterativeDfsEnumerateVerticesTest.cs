namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using Misnomer;
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
        [Theory]
        [CombinatorialData]
        public void Iterative_vertices_single_component(
            [CombinatorialValues(1, 10, 100)] int vertexCount,
            [CombinatorialValues(1.0, 1.414, 1.618, 2.0)]
            double densityPower)
        {
            // Arrange

            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(
                vertexCount, densityPower);
            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");
            int startVertex = graph.VertexCount - 1;

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            byte[] iterativeColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(iterativeColorMap, 0, iterativeColorMap.Length);

            var instantSteps = new Rist<IndexedDfsStep>(graph.VertexCount);
            var iterativeSteps = new Rist<DfsVertexStep<int>>(graph.VertexCount);
            var dfsHandler = new DfsHandler<AdjacencyListIncidenceGraph>(instantSteps);

            // Act

            InstantDfs.Traverse(graph, startVertex, instantColorMap, dfsHandler);
            iterativeSteps.AddRange(IterativeDfs.EnumerateVertices(graph, startVertex, iterativeColorMap));

            // Cleanup

            iterativeSteps.Dispose();
            instantSteps.Dispose();
            ArrayPool<byte>.Shared.Return(iterativeColorMap);
            ArrayPool<byte>.Shared.Return(instantColorMap);
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
