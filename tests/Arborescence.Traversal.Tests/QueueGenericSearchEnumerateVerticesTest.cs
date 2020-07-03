namespace Arborescence
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Misnomer;
    using Models;
    using Traversal;
    using Xunit;
    using EdgeEnumerator = ArraySegmentEnumerator<int>;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    public class QueueGenericSearchEnumerateVerticesTest
    {
        public QueueGenericSearchEnumerateVerticesTest()
        {
            InstantBfs = default;
            GenericSearch = default;
        }

        private InstantBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            InstantBfs { get; }

        private GenericSearch<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, Queue<int>, byte[],
                IndexedAdjacencyListGraphPolicy, QueuePolicy, IndexedSetPolicy>
            GenericSearch { get; }

        private void EnumerateVerticesCore(AdjacencyListIncidenceGraph graph, bool multipleSource)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            var fringe = new Queue<int>();
            byte[] enumerableExploredSet = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(enumerableExploredSet, 0, enumerableExploredSet.Length);

            using var instantSteps = new Rist<int>(graph.VertexCount);
            using var enumerableSteps = new Rist<int>(graph.VertexCount);
            BfsHandler<AdjacencyListIncidenceGraph, int, int> bfsHandler = CreateBfsHandler(instantSteps);

            // Act

            if (multipleSource)
            {
                if (graph.VertexCount < 3)
                    return;

                int sourceCount = graph.VertexCount / 3;
                var sources = new IndexEnumerator(sourceCount);

                InstantBfs.Traverse(graph, sources, instantColorMap, bfsHandler);
                using IEnumerator<int> vertices = GenericSearch.EnumerateVertices(
                    graph, sources, fringe, enumerableExploredSet);
                enumerableSteps.AddEnumerator(vertices);
            }
            else
            {
                int source = graph.VertexCount >> 1;
                InstantBfs.Traverse(graph, source, instantColorMap, bfsHandler);
                using IEnumerator<int> vertices = GenericSearch.EnumerateVertices(
                    graph, source, fringe, enumerableExploredSet);
                enumerableSteps.AddEnumerator(vertices);
            }

            // Assert

            int instantStepCount = instantSteps.Count;
            int enumerableStepCount = enumerableSteps.Count;
            Assert.Equal(instantStepCount, enumerableStepCount);

            int count = instantStepCount;
            for (int i = 0; i < count; ++i)
            {
                int instantStep = instantSteps[i];
                int enumerableStep = enumerableSteps[i];

                if (instantStep == enumerableStep)
                    continue;

                Assert.Equal(instantStep, enumerableStep);
            }

            // Cleanup

            ArrayPool<byte>.Shared.Return(instantColorMap);
            ArrayPool<byte>.Shared.Return(enumerableExploredSet);
        }

        private static BfsHandler<AdjacencyListIncidenceGraph, int, int> CreateBfsHandler(IList<int> discoveredVertices)
        {
            Debug.Assert(discoveredVertices != null, "discoveredVertices != null");

            var result = new BfsHandler<AdjacencyListIncidenceGraph, int, int>();
            result.DiscoverVertex += (g, v) => discoveredVertices.Add(v);
            return result;
        }

#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Theory]
        [ClassData(typeof(GraphCollection))]
        internal void EnumerateVertices_SingleSource(GraphParameter p)
        {
            EnumerateVerticesCore(p.Graph, false);
        }

        [Theory]
        [ClassData(typeof(GraphCollection))]
        internal void EnumerateVertices_MultipleSource(GraphParameter p)
        {
            EnumerateVerticesCore(p.Graph, true);
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
