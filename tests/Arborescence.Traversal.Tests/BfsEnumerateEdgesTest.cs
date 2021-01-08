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

    public sealed class BfsEnumerateEdgesTest
    {
        private EagerBfs<Graph, int, Endpoints, EdgeEnumerator> EagerBfs { get; }

        private EnumerableBfs<Graph, int, Endpoints, EdgeEnumerator> EnumerableBfs { get; }

        private void EnumerateEdgesCore(Graph graph, bool multipleSource)
        {
            // Arrange

            byte[] mapBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
            Array.Clear(mapBackingStore, 0, mapBackingStore.Length);
            IndexedColorDictionary eagerColorMap = new(mapBackingStore);
            byte[] setBackingStore = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
            Array.Clear(setBackingStore, 0, setBackingStore.Length);
            IndexedSet set = new(setBackingStore);

            using Rist<Endpoints> eagerSteps = new(graph.VertexCount);
            using Rist<Endpoints> enumerableSteps = new(graph.VertexCount);
            BfsHandler<Graph, int, Endpoints> bfsHandler = CreateBfsHandler(eagerSteps);

            // Act

            if (multipleSource)
            {
                if (graph.VertexCount < 3)
                    return;

                int sourceCount = graph.VertexCount / 3;
                IndexEnumerator sources = new(sourceCount);

                EagerBfs.Traverse(graph, sources, eagerColorMap, bfsHandler);
                using IEnumerator<Endpoints> edges =
                    EnumerableBfs.EnumerateEdges(graph, sources, set);
                while (edges.MoveNext())
                    enumerableSteps.Add(edges.Current);
            }
            else
            {
                int source = graph.VertexCount >> 1;
                EagerBfs.Traverse(graph, source, eagerColorMap, bfsHandler);
                using IEnumerator<Endpoints> edges = EnumerableBfs.EnumerateEdges(graph, source, set);
                while (edges.MoveNext())
                    enumerableSteps.Add(edges.Current);
            }

            // Assert

            int eagerStepCount = eagerSteps.Count;
            int enumerableStepCount = enumerableSteps.Count;
            Assert.Equal(eagerStepCount, enumerableStepCount);

            int count = eagerStepCount;
            for (int i = 0; i < count; ++i)
            {
                Endpoints eagerStep = eagerSteps[i];
                Endpoints enumerableStep = enumerableSteps[i];

                if (eagerStep == enumerableStep)
                    continue;

                Assert.Equal(eagerStep, enumerableStep);
            }

            // Cleanup

            ArrayPool<byte>.Shared.Return(mapBackingStore);
            ArrayPool<byte>.Shared.Return(setBackingStore);
        }

        private static BfsHandler<Graph, int, Endpoints> CreateBfsHandler(IList<Endpoints> treeEdges)
        {
            Debug.Assert(treeEdges != null, "treeEdges != null");

            BfsHandler<Graph, int, Endpoints> result = new();
            result.TreeEdge += (_, e) => treeEdges.Add(e);
            return result;
        }

        [Theory]
        [ClassData(typeof(UndirectedSimpleGraphCollection))]
        internal void EnumerateEdges_SingleSource(GraphParameter<Graph> p)
        {
            EnumerateEdgesCore(p.Graph, false);
        }

        [Theory]
        [ClassData(typeof(SimpleGraphCollection))]
        internal void EnumerateEdges_MultipleSource(GraphParameter<Graph> p)
        {
            EnumerateEdgesCore(p.Graph, true);
        }
    }
}
