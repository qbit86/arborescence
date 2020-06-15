namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
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
            throw new NotImplementedException();
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
