namespace Arborescence
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Misnomer;
    using Traversal;
    using Xunit;
    using EdgeEnumerator = ArraySegmentEnumerator<Endpoints>;
    using Graph = Models.SimpleIncidenceGraph;
    using GraphPolicy = Models.SimpleIncidenceGraphPolicy;

    public sealed class BfsEnumerateEdgesTest
    {
        public BfsEnumerateEdgesTest()
        {
            InstantBfs = default;
            EnumerableBfs = default;
        }

        private InstantBfs<Graph, int, Endpoints, EdgeEnumerator, byte[], GraphPolicy, IndexedColorMapPolicy>
            InstantBfs { get; }

        private EnumerableBfs<Graph, int, Endpoints, EdgeEnumerator, byte[], GraphPolicy, IndexedSetPolicy>
            EnumerableBfs { get; }

        private void EnumerateEdgesCore(Graph graph, bool multipleSource)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount >= 0, "graph.VertexCount >= 0");

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            byte[] enumerableExploredSet = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(enumerableExploredSet, 0, enumerableExploredSet.Length);

            using var instantSteps = new Rist<Endpoints>(graph.VertexCount);
            using var enumerableSteps = new Rist<Endpoints>(graph.VertexCount);
            BfsHandler<Graph, int, Endpoints> bfsHandler = CreateBfsHandler(instantSteps);

            // Act

            if (multipleSource)
            {
                if (graph.VertexCount < 3)
                    return;

                int sourceCount = graph.VertexCount / 3;
                var sources = new IndexEnumerator(sourceCount);

                InstantBfs.Traverse(graph, sources, instantColorMap, bfsHandler);
                using IEnumerator<Endpoints> edges =
                    EnumerableBfs.EnumerateEdges(graph, sources, enumerableExploredSet);
                while (edges.MoveNext())
                    enumerableSteps.Add(edges.Current);
            }
            else
            {
                int source = graph.VertexCount >> 1;
                InstantBfs.Traverse(graph, source, instantColorMap, bfsHandler);
                using IEnumerator<Endpoints> edges = EnumerableBfs.EnumerateEdges(graph, source, enumerableExploredSet);
                while (edges.MoveNext())
                    enumerableSteps.Add(edges.Current);
            }

            // Assert

            int instantStepCount = instantSteps.Count;
            int enumerableStepCount = enumerableSteps.Count;
            Assert.Equal(instantStepCount, enumerableStepCount);

            int count = instantStepCount;
            for (int i = 0; i < count; ++i)
            {
                Endpoints instantStep = instantSteps[i];
                Endpoints enumerableStep = enumerableSteps[i];

                if (instantStep == enumerableStep)
                    continue;

                Assert.Equal(instantStep, enumerableStep);
            }

            // Cleanup

            ArrayPool<byte>.Shared.Return(instantColorMap);
            ArrayPool<byte>.Shared.Return(enumerableExploredSet);
        }

        private static BfsHandler<Graph, int, Endpoints> CreateBfsHandler(IList<Endpoints> treeEdges)
        {
            Debug.Assert(treeEdges != null, "treeEdges != null");

            var result = new BfsHandler<Graph, int, Endpoints>();
            result.TreeEdge += (g, e) => treeEdges.Add(e);
            return result;
        }

#pragma warning disable CA1707 // Identifiers should not contain underscores
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
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
