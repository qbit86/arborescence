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

    public sealed class BfsEnumerateVerticesTest
    {
        private static IEnumerable<object[]> s_graphSizes;

        public BfsEnumerateVerticesTest()
        {
            InstantBfs = default;
            EnumerableBfs = default;
        }

        private InstantBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            InstantBfs { get; }

        private EnumerableBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedSetPolicy>
            EnumerableBfs { get; }

        public static IEnumerable<object[]> GraphSizes => s_graphSizes ??= GraphHelper.CreateGraphSizes();

        private void EnumerateVerticesSingleSourceCore(AdjacencyListIncidenceGraph graph)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");
            int source = graph.VertexCount - 1;

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            byte[] enumerableExploredSet = ArrayPool<byte>.Shared.Rent(
                IndexedSetPolicy.GetByteCount(graph.VertexCount));
            Array.Clear(enumerableExploredSet, 0, enumerableExploredSet.Length);

            var instantSteps = new Rist<int>(graph.VertexCount);
            var enumerableSteps = new Rist<int>(graph.VertexCount);
            BfsHandler<AdjacencyListIncidenceGraph, int, int> bfsHandler = CreateBfsHandler(instantSteps);

            // Act

            InstantBfs.Traverse(graph, source, instantColorMap, bfsHandler);
            using IEnumerator<int> vertices = EnumerableBfs.EnumerateVertices(graph, source, enumerableExploredSet);
            while (vertices.MoveNext())
                enumerableSteps.Add(vertices.Current);

            // Assert

            int instantStepCount = instantSteps.Count;
            int enumerableStepCount = enumerableSteps.Count;
            Assert.Equal(instantStepCount, enumerableStepCount);

            int count = instantStepCount;
            for (int i = 0; i < count; ++i)
            {
                int instantStep = instantSteps[i];
                int enumerableStep = enumerableSteps[i];
                Assert.Equal(instantStep, enumerableStep);
            }

            // Cleanup

            enumerableSteps.Dispose();
            instantSteps.Dispose();
            ArrayPool<byte>.Shared.Return(instantColorMap);
            ArrayPool<byte>.Shared.Return(enumerableExploredSet);
        }

        private void EnumerateVerticesMultipleSourceCore(AdjacencyListIncidenceGraph graph)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");
            if (graph.VertexCount < 3)
                return;

            int sourceCount = graph.VertexCount / 3;
            var sources = new IndexEnumerator(sourceCount);

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            byte[] enumerableExploredSet = ArrayPool<byte>.Shared.Rent(
                IndexedSetPolicy.GetByteCount(graph.VertexCount));
            Array.Clear(enumerableExploredSet, 0, enumerableExploredSet.Length);

            var instantSteps = new Rist<int>(graph.VertexCount);
            var enumerableSteps = new Rist<int>(graph.VertexCount);
            BfsHandler<AdjacencyListIncidenceGraph, int, int> bfsHandler = CreateBfsHandler(instantSteps);

            // Act

            InstantBfs.Traverse(graph, sources, instantColorMap, bfsHandler);
            using IEnumerator<int> edges = EnumerableBfs.EnumerateVertices(graph, sources, enumerableExploredSet);
            while (edges.MoveNext())
                enumerableSteps.Add(edges.Current);

            // Assert

            int instantStepCount = instantSteps.Count;
            int enumerableStepCount = enumerableSteps.Count;
            Assert.Equal(instantStepCount, enumerableStepCount);

            int count = instantStepCount;
            for (int i = 0; i < count; ++i)
            {
                int instantStep = instantSteps[i];
                int enumerableStep = enumerableSteps[i];
                Assert.Equal(instantStep, enumerableStep);
            }

            // Cleanup

            enumerableSteps.Dispose();
            instantSteps.Dispose();
            ArrayPool<byte>.Shared.Return(instantColorMap);
            ArrayPool<byte>.Shared.Return(enumerableExploredSet);
        }

        private static BfsHandler<AdjacencyListIncidenceGraph, int, int> CreateBfsHandler(IList<int> discoveredVertices)
        {
            Debug.Assert(discoveredVertices != null, "discoveredVertices != null");

            var result = new BfsHandler<AdjacencyListIncidenceGraph, int, int>();
            result.DiscoverVertex += (g, v) => discoveredVertices.Add(v);
            return result;
        }

#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Theory]
        [MemberData(nameof(GraphSizes))]
        public void Enumerate_vertices_single_source_combinatorial(int vertexCount, double densityPower)
        {
            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(
                vertexCount, densityPower);
            EnumerateVerticesSingleSourceCore(graph);
        }

        [Theory]
        [ClassData(typeof(TestCaseCollection))]
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
        [MemberData(nameof(GraphSizes))]
        public void Enumerate_vertices_multiple_source_combinatorial(int vertexCount, double densityPower)
        {
            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(
                vertexCount, densityPower);
            EnumerateVerticesMultipleSourceCore(graph);
        }

        [Theory]
        [ClassData(typeof(TestCaseCollection))]
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