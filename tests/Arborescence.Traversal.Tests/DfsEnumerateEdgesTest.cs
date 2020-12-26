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
    using EdgeEnumerator = System.ArraySegment<int>.Enumerator;
    using Graph = Models.IndexedIncidenceGraph;

    public sealed class DfsEnumerateEdgesTest
    {
        private EagerDfs<Graph, int, int, EdgeEnumerator, byte[], IndexedColorMapPolicy> EagerDfs { get; }

        private EnumerableDfs<Graph, int, int, EdgeEnumerator, byte[], IndexedSetPolicy> EnumerableDfs { get; }

        private void EnumerateEdgesCore(Graph graph, bool multipleSource)
        {
            // Arrange

            byte[] eagerColorMap = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
            Array.Clear(eagerColorMap, 0, eagerColorMap.Length);
            byte[] enumerableExploredSet = ArrayPool<byte>.Shared.Rent(Math.Max(graph.VertexCount, 1));
            Array.Clear(enumerableExploredSet, 0, enumerableExploredSet.Length);

            using var eagerSteps = new Rist<int>(graph.VertexCount);
            using var enumerableSteps = new Rist<int>(graph.VertexCount);
            DfsHandler<Graph, int, int> dfsHandler = CreateDfsHandler(eagerSteps);

            // Act

            if (multipleSource)
            {
                if (graph.VertexCount < 3)
                    return;

                int sourceCount = graph.VertexCount / 3;
                var sources = new IndexEnumerator(sourceCount);

                EagerDfs.Traverse(graph, sources, eagerColorMap, dfsHandler);
                using IEnumerator<int> edges = EnumerableDfs.EnumerateEdges(graph, sources, enumerableExploredSet);
                enumerableSteps.AddEnumerator(edges);
            }
            else
            {
                int source = graph.VertexCount >> 1;
                EagerDfs.Traverse(graph, source, eagerColorMap, dfsHandler);
                using IEnumerator<int> edges = EnumerableDfs.EnumerateEdges(graph, source, enumerableExploredSet);
                enumerableSteps.AddEnumerator(edges);
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

            ArrayPool<byte>.Shared.Return(eagerColorMap);
            ArrayPool<byte>.Shared.Return(enumerableExploredSet);
        }

        private static DfsHandler<Graph, int, int> CreateDfsHandler(IList<int> treeEdges)
        {
            Debug.Assert(treeEdges != null, "treeEdges != null");

            var result = new DfsHandler<Graph, int, int>();
            result.TreeEdge += (_, e) => treeEdges.Add(e);
            return result;
        }

        [Theory]
        [ClassData(typeof(IndexedGraphCollection))]
        internal void EnumerateEdges_SingleSource(GraphParameter<Graph> p)
        {
            EnumerateEdgesCore(p.Graph, false);
        }

        [Theory]
        [ClassData(typeof(IndexedGraphCollection))]
        internal void EnumerateEdges_MultipleSource(GraphParameter<Graph> p)
        {
            EnumerateEdgesCore(p.Graph, true);
        }
    }
}
