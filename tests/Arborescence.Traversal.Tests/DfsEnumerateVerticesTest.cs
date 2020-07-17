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

    public sealed class DfsEnumerateVerticesTest
    {
        public DfsEnumerateVerticesTest()
        {
            InstantDfs = default;
            EnumerableDfs = default;
        }

        private InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedIncidenceGraphPolicy, IndexedColorMapPolicy>
            InstantDfs { get; }

        private EnumerableDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedIncidenceGraphPolicy, IndexedSetPolicy>
            EnumerableDfs { get; }

        private void EnumerateVerticesCore(AdjacencyListIncidenceGraph graph, bool multipleSource)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            byte[] enumerableExploredSet = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(enumerableExploredSet, 0, enumerableExploredSet.Length);

            using var instantSteps = new Rist<int>(graph.VertexCount);
            using var enumerableSteps = new Rist<int>(graph.VertexCount);
            DfsHandler<AdjacencyListIncidenceGraph, int, int> dfsHandler = CreateDfsHandler(instantSteps);

            // Act

            if (multipleSource)
            {
                if (graph.VertexCount < 3)
                    return;

                int sourceCount = graph.VertexCount / 3;
                var sources = new IndexEnumerator(sourceCount);
                InstantDfs.Traverse(graph, sources, instantColorMap, dfsHandler);
                using IEnumerator<int> vertices =
                    EnumerableDfs.EnumerateVertices(graph, sources, enumerableExploredSet);
                enumerableSteps.AddEnumerator(vertices);
            }
            else
            {
                int source = graph.VertexCount - 1;
                InstantDfs.Traverse(graph, source, instantColorMap, dfsHandler);
                using IEnumerator<int> vertices = EnumerableDfs.EnumerateVertices(graph, source, enumerableExploredSet);
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

        private static DfsHandler<AdjacencyListIncidenceGraph, int, int> CreateDfsHandler(IList<int> steps)
        {
            Debug.Assert(steps != null, "steps != null");

            var result = new DfsHandler<AdjacencyListIncidenceGraph, int, int>();
            result.DiscoverVertex += (g, v) => steps.Add(v);
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
