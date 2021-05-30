namespace Arborescence
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Misnomer;
    using Traversal;
    using Xunit;
    using EdgeEnumerator = System.ArraySegment<int>.Enumerator;
    using Graph = Models.IndexedIncidenceGraph;

    public sealed class DfsEnumerateVerticesTest
    {
        private EagerDfs<Graph, int, int, EdgeEnumerator> EagerDfs { get; }

        private EnumerableDfs<Graph, int, int, EdgeEnumerator> EnumerableDfs { get; }

        private void EnumerateVerticesCore(Graph graph, bool multipleSource)
        {
            // Arrange

            if (graph.VertexCount == 0)
                return;

            byte[] mapBackingStore = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(mapBackingStore, 0, mapBackingStore.Length);
            IndexedColorDictionary eagerColorMap = new(mapBackingStore);
            byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(setBackingStore, 0, setBackingStore.Length);
            IndexedSet set = new(setBackingStore);

            using Rist<int> eagerSteps = new(graph.VertexCount);
            using Rist<int> enumerableSteps = new(graph.VertexCount);
            DfsHandler<Graph, int, int> dfsHandler = CreateDfsHandler(eagerSteps);

            // Act

            if (multipleSource)
            {
                if (graph.VertexCount < 3)
                    return;

                int sourceCount = graph.VertexCount / 3;
                IndexEnumerator sources = new(sourceCount);
                EagerDfs.Traverse(graph, sources, eagerColorMap, dfsHandler);
                using IEnumerator<int> vertices = EnumerableDfs.EnumerateVertices(graph, sources, set);
                enumerableSteps.AddEnumerator(vertices);
            }
            else
            {
                int source = graph.VertexCount - 1;
                EagerDfs.Traverse(graph, source, eagerColorMap, dfsHandler);
                using IEnumerator<int> vertices = EnumerableDfs.EnumerateVertices(graph, source, set);
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

        private static DfsHandler<Graph, int, int> CreateDfsHandler(IList<int> steps)
        {
            Debug.Assert(steps != null, "steps != null");

            DfsHandler<Graph, int, int> result = new();
            result.DiscoverVertex += (_, v) => steps.Add(v);
            return result;
        }

        [Theory]
        [ClassData(typeof(FromMutableIndexedGraphCollection))]
        internal void EnumerateVertices_SingleSource(GraphParameter<Graph> p)
        {
            EnumerateVerticesCore(p.Graph, false);
        }

        [Theory]
        [ClassData(typeof(FromMutableIndexedGraphCollection))]
        internal void EnumerateVertices_MultipleSource(GraphParameter<Graph> p)
        {
            EnumerateVerticesCore(p.Graph, true);
        }
    }
}
