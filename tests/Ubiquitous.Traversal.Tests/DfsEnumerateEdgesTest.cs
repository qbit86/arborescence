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

    public sealed class DfsEnumerateEdgesTest
    {
        public DfsEnumerateEdgesTest()
        {
            InstantDfs = default;
            EnumerableDfs = default;
        }

        private InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            InstantDfs { get; }

        private EnumerableDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedSetPolicy>
            EnumerableDfs { get; }

        private void EnumerateEdgesCore(AdjacencyListIncidenceGraph graph, bool multipleSource)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            byte[] enumerableExploredSet = ArrayPool<byte>.Shared.Rent(
                IndexedSetPolicy.GetByteCount(graph.VertexCount));
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
                using IEnumerator<int> edges = EnumerableDfs.EnumerateEdges(graph, sources, enumerableExploredSet);
                enumerableSteps.AddEnumerator(edges);
            }
            else
            {
                int source = graph.VertexCount >> 1;
                InstantDfs.Traverse(graph, source, instantColorMap, dfsHandler);
                using IEnumerator<int> edges = EnumerableDfs.EnumerateEdges(graph, source, enumerableExploredSet);
                enumerableSteps.AddEnumerator(edges);
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

        private static DfsHandler<AdjacencyListIncidenceGraph, int, int> CreateDfsHandler(IList<int> treeEdges)
        {
            Debug.Assert(treeEdges != null, "treeEdges != null");

            var result = new DfsHandler<AdjacencyListIncidenceGraph, int, int>();
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
            EnumerateEdgesCore(graph, false);
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
            EnumerateEdgesCore(graph, false);
        }

        [Theory(Skip = "Not implemented yet")]
        [ClassData(typeof(GraphSizeCollection))]
        public void EnumerateEdges_MultipleSource_ByGraphSize(int vertexCount, double densityPower)
        {
            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(
                vertexCount, densityPower);
            EnumerateEdgesCore(graph, true);
        }

        [Theory(Skip = "Not implemented yet")]
        [ClassData(typeof(TestCaseCollection))]
        public void EnumerateEdges_MultipleSource_ByTestCase(string testCase)
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
            EnumerateEdgesCore(graph, true);
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
