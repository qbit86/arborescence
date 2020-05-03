namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Misnomer;
    using Models;
    using Traversal;
    using Workbench;
    using Xunit;
    using EdgeEnumerator = ArraySegmentEnumerator<int>;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    public sealed class IterativeDfsEnumerateVerticesTest
    {
        private static IEnumerable<object[]> s_testCases;

        public IterativeDfsEnumerateVerticesTest()
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

        public static IEnumerable<object[]> TestCases => s_testCases ??= CreateTestCases();

        private static IEnumerable<object[]> CreateTestCases()
        {
            return Enumerable.Range(1, 7).Select(it => new object[] { it });
        }

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

            var instantSteps = new Rist<DfsVertexStep<int>>(graph.VertexCount);
            var iterativeSteps = new Rist<DfsVertexStep<int>>(graph.VertexCount);
            var dfsHandler = new IndexedDfsVertexHandler<AdjacencyListIncidenceGraph>(instantSteps);

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
                DfsVertexStep<int> instantStep = instantSteps[i];
                DfsVertexStep<int> iterativeStep = iterativeSteps[i];
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
        public void Iterative_vertices_single_component_member(int testCase)
        {
            var builder = new AdjacencyListIncidenceGraphBuilder(10);

            string name = testCase.ToString("D2", CultureInfo.InvariantCulture);
            using (TextReader textReader = IndexedGraphs.GetTextReader(name))
            {
                Assert.NotEqual(TextReader.Null, textReader);
                IEnumerable<SourceTargetPair<int>> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (SourceTargetPair<int> edge in edges)
                    builder.TryAdd(edge.Source, edge.Target, out _);
            }

            AdjacencyListIncidenceGraph graph = builder.ToGraph();
            IterativeVerticesSingleComponentCore(graph);
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
