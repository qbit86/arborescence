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

            IterativeDfs = IterativeDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[]>.Create(
                graphPolicy, colorMapPolicy);
        }

        private InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            InstantDfs { get; }

        private IterativeDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            IterativeDfs { get; }

        public static IEnumerable<object[]> TestCases => s_testCases ??= GraphHelper.CreateTestCases();

        private void IterativeVerticesSingleComponentCore(AdjacencyListIncidenceGraph graph)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");
            int startVertex = graph.VertexCount - 1;

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            byte[] iterativeColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(iterativeColorMap, 0, iterativeColorMap.Length);

            var instantSteps = new Rist<DfsStep<int>>(graph.VertexCount);
            var iterativeSteps = new Rist<DfsStep<int>>(graph.VertexCount);
            DfsHandler<AdjacencyListIncidenceGraph, int, int> dfsHandler = CreateDfsHandler(instantSteps);

            // Act

            InstantDfs.Traverse(graph, startVertex, instantColorMap, dfsHandler);
            iterativeSteps.AddRange(IterativeDfs.EnumerateVertices(graph, startVertex, iterativeColorMap));

            // Assert

            int instantStepCount = instantSteps.Count;
            int iterativeStepCount = iterativeSteps.Count;
            Assert.Equal(instantStepCount, iterativeStepCount);

            int count = instantStepCount;
            for (int i = 0; i != count; ++i)
            {
                DfsStep<int> instantStep = instantSteps[i];
                DfsStep<int> iterativeStep = iterativeSteps[i];
                Assert.NotEqual(DfsStepKind.None, iterativeStep.Kind);

                if (instantStep == iterativeStep)
                    continue;

                Assert.Equal(instantStep, iterativeStep);
            }

            // Cleanup

            iterativeSteps.Dispose();
            instantSteps.Dispose();
            ArrayPool<byte>.Shared.Return(iterativeColorMap);
            ArrayPool<byte>.Shared.Return(instantColorMap);
        }

        private void IterativeVerticesCrossComponentCore(AdjacencyListIncidenceGraph graph)
        {
            Debug.Assert(graph != null, "graph != null");

            // Arrange

            Debug.Assert(graph.VertexCount > 0, "graph.VertexCount > 0");
            int startVertex = graph.VertexCount - 1;
            var vertices = new RangeEnumerator(0, graph.VertexCount);

            byte[] instantColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(instantColorMap, 0, instantColorMap.Length);
            byte[] iterativeColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(iterativeColorMap, 0, iterativeColorMap.Length);

            var instantSteps = new Rist<DfsStep<int>>(graph.VertexCount);
            var iterativeSteps = new Rist<DfsStep<int>>(graph.VertexCount);
            DfsHandler<AdjacencyListIncidenceGraph, int, int> dfsHandler = CreateDfsHandler(instantSteps);

            // Act

            InstantDfs.Traverse(graph, vertices, instantColorMap, dfsHandler, startVertex);
            IEnumerator<DfsStep<int>> stepEnumerator = IterativeDfs.EnumerateVertices(
                graph, vertices, iterativeColorMap, startVertex);
            while (stepEnumerator.MoveNext())
                iterativeSteps.Add(stepEnumerator.Current);

            // Assert

            int instantStepCount = instantSteps.Count;
            int iterativeStepCount = iterativeSteps.Count;
            Assert.Equal(instantStepCount, iterativeStepCount);

            int count = instantStepCount;
            for (int i = 0; i != count; ++i)
            {
                DfsStep<int> instantStep = instantSteps[i];
                DfsStep<int> iterativeStep = iterativeSteps[i];
                Assert.NotEqual(DfsStepKind.None, iterativeStep.Kind);

                if (instantStep == iterativeStep)
                    continue;

                Assert.Equal(instantStep, iterativeStep);
            }

            // Cleanup

            iterativeSteps.Dispose();
            instantSteps.Dispose();
            ArrayPool<byte>.Shared.Return(iterativeColorMap);
            ArrayPool<byte>.Shared.Return(instantColorMap);
        }

        private DfsHandler<AdjacencyListIncidenceGraph, int, int> CreateDfsHandler(IList<DfsStep<int>> steps)
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
        public void Iterative_vertices_single_component_combinatorial(
            [CombinatorialValues(1, 10, 100)] int vertexCount,
            [CombinatorialValues(1.0, 1.414, 1.618, 2.0)]
            double densityPower)
        {
            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(
                vertexCount, densityPower);
            IterativeVerticesSingleComponentCore(graph);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Iterative_vertices_single_component_member(string testCase)
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
            IterativeVerticesSingleComponentCore(graph);
        }

        [Theory]
        [CombinatorialData]
        public void Iterative_vertices_cross_component_combinatorial(
            [CombinatorialValues(1, 10, 100)] int vertexCount,
            [CombinatorialValues(1.0, 1.414, 1.618, 2.0)]
            double densityPower)
        {
            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(
                vertexCount, densityPower);
            IterativeVerticesCrossComponentCore(graph);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Iterative_vertices_cross_component_member(string testCase)
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
            IterativeVerticesCrossComponentCore(graph);
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
