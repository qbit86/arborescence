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

    public sealed class DfsEnumerateVerticesTest
    {
        private static IEnumerable<object[]> s_testCases;

        public DfsEnumerateVerticesTest()
        {
            var graphPolicy = default(IndexedAdjacencyListGraphPolicy);
            var colorMapPolicy = default(IndexedColorMapPolicy);

            InstantDfs = InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[]>.Create(
                graphPolicy, colorMapPolicy);

            EnumerableDfs = EnumerableDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[]>.Create(
                graphPolicy, colorMapPolicy);
        }

        private InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            InstantDfs { get; }

        private EnumerableDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            EnumerableDfs { get; }

        public static IEnumerable<object[]> TestCases => s_testCases ??= GraphHelper.CreateTestCases();

        private void EnumerableVerticesSingleComponentCore(AdjacencyListIncidenceGraph graph)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");
            int startVertex = graph.VertexCount - 1;

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            byte[] enumerableColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(enumerableColorMap, 0, enumerableColorMap.Length);

            var instantSteps = new Rist<DfsStep<int>>(graph.VertexCount);
            var enumerableSteps = new Rist<DfsStep<int>>(graph.VertexCount);
            DfsHandler<AdjacencyListIncidenceGraph, int, int> dfsHandler = CreateDfsHandler(instantSteps);

            // Act

            InstantDfs.Traverse(graph, startVertex, instantColorMap, dfsHandler);
            enumerableSteps.AddRange(EnumerableDfs.EnumerateVertices(graph, startVertex, enumerableColorMap));

            // Assert

            int instantStepCount = instantSteps.Count;
            int enumerableStepCount = enumerableSteps.Count;
            Assert.Equal(instantStepCount, enumerableStepCount);

            int count = instantStepCount;
            for (int i = 0; i != count; ++i)
            {
                DfsStep<int> instantStep = instantSteps[i];
                DfsStep<int> enumerableStep = enumerableSteps[i];
                Assert.NotEqual(DfsStepKind.None, enumerableStep.Kind);

                if (instantStep == enumerableStep)
                    continue;

                Assert.Equal(instantStep, enumerableStep);
            }

            // Cleanup

            enumerableSteps.Dispose();
            instantSteps.Dispose();
            ArrayPool<byte>.Shared.Return(enumerableColorMap);
            ArrayPool<byte>.Shared.Return(instantColorMap);
        }

        private void EnumerableVerticesCrossComponentCore(AdjacencyListIncidenceGraph graph)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");
            int startVertex = graph.VertexCount - 1;
            var vertices = new IndexEnumerator(graph.VertexCount);

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            byte[] enumerableColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(enumerableColorMap, 0, enumerableColorMap.Length);

            var instantSteps = new Rist<DfsStep<int>>(graph.VertexCount);
            var enumerableSteps = new Rist<DfsStep<int>>(graph.VertexCount);
            DfsHandler<AdjacencyListIncidenceGraph, int, int> dfsHandler = CreateDfsHandler(instantSteps);

            // Act

            InstantDfs.Traverse(graph, vertices, instantColorMap, dfsHandler, startVertex);
            IEnumerator<DfsStep<int>> stepEnumerator = EnumerableDfs.EnumerateVertices(
                graph, vertices, enumerableColorMap, startVertex);
            while (stepEnumerator.MoveNext())
                enumerableSteps.Add(stepEnumerator.Current);

            // Assert

            int instantStepCount = instantSteps.Count;
            int enumerableStepCount = enumerableSteps.Count;
            Assert.Equal(instantStepCount, enumerableStepCount);

            int count = instantStepCount;
            for (int i = 0; i != count; ++i)
            {
                DfsStep<int> instantStep = instantSteps[i];
                DfsStep<int> enumerableStep = enumerableSteps[i];
                Assert.NotEqual(DfsStepKind.None, enumerableStep.Kind);

                if (instantStep == enumerableStep)
                    continue;

                Assert.Equal(instantStep, enumerableStep);
            }

            // Cleanup

            enumerableSteps.Dispose();
            instantSteps.Dispose();
            ArrayPool<byte>.Shared.Return(enumerableColorMap);
            ArrayPool<byte>.Shared.Return(instantColorMap);
        }

        private static DfsHandler<AdjacencyListIncidenceGraph, int, int> CreateDfsHandler(IList<DfsStep<int>> steps)
        {
            Debug.Assert(steps != null, "steps != null");

            var result = new DfsHandler<AdjacencyListIncidenceGraph, int, int>();
            result.StartVertex += (g, v) => steps.Add(DfsStep.Create(DfsStepKind.StartVertex, v));
            result.DiscoverVertex += (g, v) => steps.Add(DfsStep.Create(DfsStepKind.DiscoverVertex, v));
            result.FinishVertex += (g, v) => steps.Add(DfsStep.Create(DfsStepKind.FinishVertex, v));
            return result;
        }

#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Theory]
        [CombinatorialData]
        public void Enumerable_vertices_single_component_combinatorial(
            [CombinatorialValues(1, 10, 100)] int vertexCount,
            [CombinatorialValues(1.0, 1.414, 1.618, 2.0)]
            double densityPower)
        {
            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(
                vertexCount, densityPower);
            EnumerableVerticesSingleComponentCore(graph);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Enumerable_vertices_single_component_member(string testCase)
        {
            Assert.NotNull(testCase);
            var builder = new AdjacencyListIncidenceGraphBuilder(10);

            using (TextReader textReader = IndexedGraphs.GetTextReader(testCase))
            {
                Assert.NotEqual(TextReader.Null, textReader);
                IEnumerable<SourceTargetPair<int>> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (SourceTargetPair<int> edge in edges)
                    builder.TryAdd(edge.Source, edge.Target, out _);
            }

            AdjacencyListIncidenceGraph graph = builder.ToGraph();
            EnumerableVerticesSingleComponentCore(graph);
        }

        [Theory]
        [CombinatorialData]
        public void Enumerable_vertices_cross_component_combinatorial(
            [CombinatorialValues(1, 10, 100)] int vertexCount,
            [CombinatorialValues(1.0, 1.414, 1.618, 2.0)]
            double densityPower)
        {
            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(
                vertexCount, densityPower);
            EnumerableVerticesCrossComponentCore(graph);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Enumerable_vertices_cross_component_member(string testCase)
        {
            Assert.NotNull(testCase);
            var builder = new AdjacencyListIncidenceGraphBuilder(10);

            using (TextReader textReader = IndexedGraphs.GetTextReader(testCase))
            {
                Assert.NotEqual(TextReader.Null, textReader);
                IEnumerable<SourceTargetPair<int>> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (SourceTargetPair<int> edge in edges)
                    builder.TryAdd(edge.Source, edge.Target, out _);
            }

            AdjacencyListIncidenceGraph graph = builder.ToGraph();
            EnumerableVerticesCrossComponentCore(graph);
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}