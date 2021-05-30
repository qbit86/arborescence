namespace Arborescence
{
    using System;
    using System.Buffers;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Misnomer;
    using Traversal;
    using Xunit;
    using EdgeEnumerator = System.ArraySegment<Endpoints>.Enumerator;
    using Graph = Models.MutableSimpleIncidenceGraph;

    public class QueueGenericSearchEnumerateVerticesTest
    {
        private EagerBfs<Graph, int, Endpoints, EdgeEnumerator> EagerBfs { get; }

        private GenericSearch<Graph, int, Endpoints, EdgeEnumerator> GenericSearch { get; }

        private void EnumerateVerticesCore(Graph graph, bool multipleSource)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            byte[] mapBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
            Array.Clear(mapBackingStore, 0, mapBackingStore.Length);
            IndexedColorDictionary eagerColorMap = new(mapBackingStore);
            ConcurrentQueue<int> fringe = new();
            byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
            Array.Clear(setBackingStore, 0, setBackingStore.Length);
            IndexedSet set = new(setBackingStore);

            using Rist<int> eagerSteps = new(graph.VertexCount);
            using Rist<int> enumerableSteps = new(graph.VertexCount);
            BfsHandler<Graph, int, Endpoints> bfsHandler = CreateBfsHandler(eagerSteps);

            // Act

            if (multipleSource)
            {
                if (graph.VertexCount < 3)
                    return;

                int sourceCount = graph.VertexCount / 3;
                IndexEnumerator sources = new(sourceCount);

                EagerBfs.Traverse(graph, sources, eagerColorMap, bfsHandler);
                using IEnumerator<int> vertices = GenericSearch.EnumerateVertices(graph, sources, fringe, set);
                enumerableSteps.AddEnumerator(vertices);
            }
            else
            {
                int source = graph.VertexCount >> 1;
                EagerBfs.Traverse(graph, source, eagerColorMap, bfsHandler);
                using IEnumerator<int> vertices = GenericSearch.EnumerateVertices(graph, source, fringe, set);
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

                if (eagerStep == enumerableStep)
                    continue;

                Assert.Equal(eagerStep, enumerableStep);
            }

            // Cleanup

            ArrayPool<byte>.Shared.Return(mapBackingStore);
            ArrayPool<byte>.Shared.Return(setBackingStore);
        }

        private static BfsHandler<Graph, int, Endpoints> CreateBfsHandler(IList<int> discoveredVertices)
        {
            Debug.Assert(discoveredVertices != null, "discoveredVertices != null");

            BfsHandler<Graph, int, Endpoints> result = new();
            result.DiscoverVertex += (_, v) => discoveredVertices.Add(v);
            return result;
        }

        [Theory]
        [ClassData(typeof(MutableSimpleGraphCollection))]
        internal void EnumerateVertices_SingleSource(GraphParameter<Graph> p)
        {
            EnumerateVerticesCore(p.Graph, false);
        }

        [Theory]
        [ClassData(typeof(MutableSimpleGraphCollection))]
        internal void EnumerateVertices_MultipleSource(GraphParameter<Graph> p)
        {
            EnumerateVerticesCore(p.Graph, true);
        }
    }
}
