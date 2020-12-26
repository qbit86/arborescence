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
    using EdgeEnumerator = System.ArraySegment<Endpoints>.Enumerator;
    using Graph = Models.SimpleIncidenceGraph;

    public sealed class BfsEnumerateVerticesTest
    {
        private EagerBfs<Graph, int, Endpoints, EdgeEnumerator, byte[], IndexedColorMapPolicy> EagerBfs { get; }

        private EnumerableBfs<Graph, int, Endpoints, EdgeEnumerator, byte[], IndexedSetPolicy> EnumerableBfs { get; }

        private void EnumerateVerticesCore(Graph graph, bool multipleSource)
        {
            // Arrange

            byte[] eagerColorMap = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
            Array.Clear(eagerColorMap, 0, eagerColorMap.Length);
            byte[] enumerableExploredSet = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
            Array.Clear(enumerableExploredSet, 0, enumerableExploredSet.Length);

            using var eagerSteps = new Rist<int>(graph.VertexCount);
            using var enumerableSteps = new Rist<int>(graph.VertexCount);
            BfsHandler<Graph, int, Endpoints> bfsHandler = CreateBfsHandler(eagerSteps);

            // Act

            if (multipleSource)
            {
                if (graph.VertexCount < 3)
                    return;

                int sourceCount = graph.VertexCount / 3;
                var sources = new IndexEnumerator(sourceCount);

                EagerBfs.Traverse(graph, sources, eagerColorMap, bfsHandler);
                using IEnumerator<int> vertices =
                    EnumerableBfs.EnumerateVertices(graph, sources, enumerableExploredSet);
                enumerableSteps.AddEnumerator(vertices);
            }
            else
            {
                int source = graph.VertexCount >> 1;
                EagerBfs.Traverse(graph, source, eagerColorMap, bfsHandler);
                using IEnumerator<int> vertices = EnumerableBfs.EnumerateVertices(graph, source, enumerableExploredSet);
                enumerableSteps.AddEnumerator(vertices);
            }

            // Assert

            int eagerStepCount = eagerSteps.Count;
            int enumerableStepCount = enumerableSteps.Count;
            Assert.Equal(eagerStepCount, enumerableStepCount);

            int count = eagerStepCount;
            for (int i = 0; i < count; ++i)
            {
                int eagerStep = eagerSteps[i];
                int enumerableStep = enumerableSteps[i];
                Assert.Equal(eagerStep, enumerableStep);
            }

            // Cleanup

            ArrayPool<byte>.Shared.Return(eagerColorMap);
            ArrayPool<byte>.Shared.Return(enumerableExploredSet);
        }

        private static BfsHandler<Graph, int, Endpoints> CreateBfsHandler(IList<int> discoveredVertices)
        {
            Debug.Assert(discoveredVertices != null, "discoveredVertices != null");

            var result = new BfsHandler<Graph, int, Endpoints>();
            result.DiscoverVertex += (_, v) => discoveredVertices.Add(v);
            return result;
        }

        [Theory]
        [ClassData(typeof(FromMutableSimpleGraphCollection))]
        internal void EnumerateVertices_SingleSource(GraphParameter<Graph> p)
        {
            EnumerateVerticesCore(p.Graph, false);
        }

        [Theory]
        [ClassData(typeof(FromMutableSimpleGraphCollection))]
        internal void EnumerateVertices_MultipleSource(GraphParameter<Graph> p)
        {
            EnumerateVerticesCore(p.Graph, true);
        }
    }
}
