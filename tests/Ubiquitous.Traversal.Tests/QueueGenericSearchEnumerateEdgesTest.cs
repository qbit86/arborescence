namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Misnomer;
    using Models;
    using Traversal;
    using Workbench;
    using Xunit;
    using EdgeEnumerator = ArraySegmentEnumerator<int>;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    public sealed class QueueGenericSearchEnumerateEdgesTest
    {
        public QueueGenericSearchEnumerateEdgesTest()
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

        private void EnumerateEdgesSingleSourceCore(AdjacencyListIncidenceGraph graph)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");
            int source = graph.VertexCount >> 1;

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            var container = new Queue<int>();
            byte[] enumerableExploredSet = ArrayPool<byte>.Shared.Rent(
                IndexedSetPolicy.GetByteCount(graph.VertexCount));
            Array.Clear(enumerableExploredSet, 0, enumerableExploredSet.Length);

            var instantSteps = new Rist<int>(graph.VertexCount);
            var enumerableSteps = new Rist<int>(graph.VertexCount);
            BfsHandler<AdjacencyListIncidenceGraph, int, int> bfsHandler = CreateBfsHandler(instantSteps);

            // Act

            InstantBfs.Traverse(graph, source, instantColorMap, bfsHandler);
            using IEnumerator<int> edges = GenericSearch.EnumerateEdges(
                graph, source, container, enumerableExploredSet);
            enumerableSteps.AddEnumerator(edges);

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

            enumerableSteps.Dispose();
            instantSteps.Dispose();
            ArrayPool<byte>.Shared.Return(instantColorMap);
            ArrayPool<byte>.Shared.Return(enumerableExploredSet);
        }

        private static BfsHandler<AdjacencyListIncidenceGraph, int, int> CreateBfsHandler(IList<int> treeEdges)
        {
            Debug.Assert(treeEdges != null, "treeEdges != null");

            var result = new BfsHandler<AdjacencyListIncidenceGraph, int, int>();
            result.TreeEdge += (g, e) => treeEdges.Add(e);
            return result;
        }

#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Theory]
        [ClassData(typeof(GraphSizeCollection))]
        public void EnumerateEdges_SingleSource_ByGraphSize(int vertexCount, double densityPower)
        {
            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(
                vertexCount, densityPower);
            EnumerateEdgesSingleSourceCore(graph);
        }

        [Theory]
        [ClassData(typeof(TestCaseCollection))]
        public void EnumerateEdges_SingleSource_ByTestCase(string testCase)
        {
            Assert.NotNull(testCase);
            var builder = new AdjacencyListIncidenceGraphBuilder(10);

            using (TextReader textReader = IndexedGraphs.GetTextReader(testCase))
            {
                Assert.NotEqual(TextReader.Null, textReader);
                IEnumerable<Endpoints<int>> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (Endpoints<int> edge in edges)
                    builder.TryAdd(edge.Tail, edge.Head, out _);
            }

            AdjacencyListIncidenceGraph graph = builder.ToGraph();
            EnumerateEdgesSingleSourceCore(graph);
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }

    internal readonly struct QueuePolicy : IContainerPolicy<Queue<int>, int>
    {
        public void Add(Queue<int> container, int item) => container.Enqueue(item);

        public bool TryTake(Queue<int> container, out int result) => container.TryDequeue(out result);
    }
}
