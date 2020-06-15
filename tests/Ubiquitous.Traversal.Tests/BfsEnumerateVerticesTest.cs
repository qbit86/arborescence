namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using System.Collections;
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

    public sealed class BfsEnumerateVerticesTest
    {
        private static IEnumerable<object[]> s_testCases;

        public BfsEnumerateVerticesTest()
        {
            IndexedAdjacencyListGraphPolicy graphPolicy = default;
            IndexedColorMapPolicy colorMapPolicy = default;

            InstantBfs = InstantBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[]>.Create(
                graphPolicy, colorMapPolicy);

            IndexedSetPolicy exploredSetPolicy = default;
            EnumerableBfs = EnumerableBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, BitArray>.Create(
                graphPolicy, exploredSetPolicy);
        }

        private InstantBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            InstantBfs { get; }

        private EnumerableBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, BitArray,
                IndexedAdjacencyListGraphPolicy, IndexedSetPolicy>
            EnumerableBfs { get; }

        public static IEnumerable<object[]> TestCases => s_testCases ??= GraphHelper.CreateTestCases();

        private void EnumerateVerticesSingleSourceCore(AdjacencyListIncidenceGraph graph)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");
            int source = graph.VertexCount - 1;

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            var enumerableExploredSet = new BitArray(graph.VertexCount);

            var instantSteps = new Rist<int>(graph.VertexCount);
            var enumerableSteps = new Rist<int>(graph.VertexCount);
            BfsHandler<AdjacencyListIncidenceGraph, int, int> bfsHandler = CreateBfsHandler(instantSteps);

            // Act

            InstantBfs.Traverse(graph, source, instantColorMap, bfsHandler);
            IEnumerator<int> vertices = EnumerableBfs.EnumerateVertices(graph, source, enumerableExploredSet);
            while (vertices.MoveNext())
                enumerableSteps.Add(vertices.Current);

            // Assert

            int instantStepCount = instantSteps.Count;
            int enumerableStepCount = enumerableSteps.Count;
            Assert.Equal(instantStepCount, enumerableStepCount);

            int count = instantStepCount;
            for (int i = 0; i != count; ++i)
            {
                int instantStep = instantSteps[i];
                int enumerableStep = enumerableSteps[i];

                Assert.True(graph.TryGetHead(instantStep, out int head));

                if (head == enumerableStep)
                    continue;

                Assert.Equal(instantStep, enumerableStep);
            }

            // Cleanup

            enumerableSteps.Dispose();
            instantSteps.Dispose();
            ArrayPool<byte>.Shared.Return(instantColorMap);
        }

        private void EnumerateVerticesMultipleSourceCore(AdjacencyListIncidenceGraph graph)
        {
            throw new NotImplementedException();
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
        [CombinatorialData]
        public void Enumerate_vertices_single_source_combinatorial(
            [CombinatorialValues(1, 10, 100)] int vertexCount,
            [CombinatorialValues(1.0, 1.414, 1.618, 2.0)]
            double densityPower)
        {
            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(
                vertexCount, densityPower);
            EnumerateVerticesSingleSourceCore(graph);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Enumerate_vertices_single_source_member(string testCase)
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
            EnumerateVerticesSingleSourceCore(graph);
        }

        [Theory]
        [CombinatorialData]
        public void Enumerate_vertices_multiple_source_combinatorial(
            [CombinatorialValues(1, 10, 100)] int vertexCount,
            [CombinatorialValues(1.0, 1.414, 1.618, 2.0)]
            double densityPower)
        {
            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(
                vertexCount, densityPower);
            EnumerateVerticesMultipleSourceCore(graph);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Enumerate_vertices_multiple_source_member(string testCase)
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
            EnumerateVerticesMultipleSourceCore(graph);
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
