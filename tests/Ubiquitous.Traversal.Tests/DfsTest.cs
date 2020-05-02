namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using System.Globalization;
    using System.Linq;
    using Misnomer;
    using Models;
    using Traversal;
    using Xunit;
    using Xunit.Abstractions;
    using EdgeEnumerator = ArraySegmentEnumerator<int>;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    public sealed class DfsTest
    {
        public DfsTest(ITestOutputHelper output)
        {
            var colorMapPolicy = default(IndexedColorMapPolicy);

            Dfs = Dfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[], IndexedDfsStep>
                .Create(default(IndexedAdjacencyListGraphPolicy), colorMapPolicy, default(IndexedDfsStepPolicy));

            MultipleSourceDfs = MultipleSourceDfs<AdjacencyListIncidenceGraph, int, int, IndexCollection,
                IndexCollectionEnumerator, EdgeEnumerator, byte[], IndexedDfsStep>.Create(
                default(IndexedAdjacencyListGraphPolicy), colorMapPolicy, default(IndexCollectionEnumerablePolicy),
                default(IndexedDfsStepPolicy));

            InstantDfs = InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[]>
                .Create(default(IndexedAdjacencyListGraphPolicy), colorMapPolicy);

            Output = output;
        }

        private static CultureInfo F => CultureInfo.InvariantCulture;

        private Dfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[], IndexedDfsStep,
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy, IndexedDfsStepPolicy>
            Dfs { get; }

        private MultipleSourceDfs<AdjacencyListIncidenceGraph, int, int,
                IndexCollection, IndexCollectionEnumerator, EdgeEnumerator,
                byte[], IndexedDfsStep,
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy,
                IndexCollectionEnumerablePolicy, IndexedDfsStepPolicy>
            MultipleSourceDfs { get; }

        private InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            InstantDfs { get; }

        private ITestOutputHelper Output { get; }

#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Theory, CombinatorialData]
        public void Baseline_and_boost_implementations_should_match_for_tree(
            [CombinatorialValues(1, 10, 100)] int vertexCount,
            [CombinatorialValues(1.0, 1.414, 1.618, 2.0)] double densityPower)
        {
            // Arrange

            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(
                vertexCount, densityPower);
            int vertex = 0;
            int stepCountApproximation = graph.VertexCount + graph.EdgeCount;

            byte[] boostColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(boostColorMap, 0, boostColorMap.Length);
            byte[] colorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            var instantSteps = new Rist<IndexedDfsStep>(graph.VertexCount);
            var dfsHandler = new DfsHandler<AdjacencyListIncidenceGraph>(instantSteps);

            // Act

            Rist<IndexedDfsStep> boostSteps = RistFactory<IndexedDfsStep>.Create(
                Dfs.Traverse(graph, vertex, boostColorMap).GetEnumerator(), stepCountApproximation);
            InstantDfs.Traverse(graph, vertex, colorMap, dfsHandler);

            // Assert

            int instantStepCount = instantSteps.Count;
            int boostStepCount = boostSteps.Count;
            if (instantStepCount != boostStepCount)
            {
                Output.WriteLine(
                    $"{nameof(instantStepCount)}: {instantStepCount.ToString(F)}, {nameof(boostStepCount)}: {boostStepCount.ToString(F)}");
            }

            Assert.Equal(instantStepCount, boostStepCount);

            int count = Math.Min(instantStepCount, boostStepCount);
            for (int i = 0; i != count; ++i)
            {
                IndexedDfsStep baselineStep = instantSteps[i];
                IndexedDfsStep boostStep = boostSteps[i];

                if (baselineStep == boostStep)
                    continue;

                Output.WriteLine(
                    $"{nameof(i)}: {i}, {nameof(baselineStep)}: {baselineStep.ToString()}, {nameof(boostStep)}: {boostStep.ToString()}");
            }

            Assert.Equal(instantSteps, boostSteps, IndexedDfsStepEqualityComparer.Default);

            boostSteps.Dispose();
            instantSteps.Dispose();
            ArrayPool<byte>.Shared.Return(colorMap);
        }

        [Theory, CombinatorialData]
        public void Baseline_and_boost_implementations_should_match_for_forest(
            [CombinatorialValues(1, 10, 100)] int vertexCount,
            [CombinatorialValues(1.0, 1.414, 1.618, 2.0)] double densityPower)
        {
            // Arrange

            AdjacencyListIncidenceGraph graph = GraphHelper.GenerateAdjacencyListIncidenceGraph(
                vertexCount, densityPower);
            var vertices = new IndexCollection(graph.VertexCount);
            int stepCountApproximation = graph.VertexCount + graph.EdgeCount;

            byte[] boostColorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(boostColorMap, 0, boostColorMap.Length);
            byte[] colorMap = ArrayPool<byte>.Shared.Rent(graph.VertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            var instantSteps = new Rist<IndexedDfsStep>(graph.VertexCount);
            var dfsHandler = new DfsHandler<AdjacencyListIncidenceGraph>(instantSteps);

            // Act

            Rist<IndexedDfsStep> boostSteps = RistFactory<IndexedDfsStep>.Create(
                MultipleSourceDfs.Traverse(graph, vertices, boostColorMap).GetEnumerator(), stepCountApproximation);
            InstantDfs.Traverse(graph, vertices.Enumerate(), colorMap, dfsHandler);

            int discoveredVertexCount = boostSteps.Count(s => s.Kind == DfsStepKind.DiscoverVertex);
            int expectedStartVertexCount = instantSteps.Count(s => s.Kind == DfsStepKind.StartVertex);
            int actualStartVertexCount = boostSteps.Count(s => s.Kind == DfsStepKind.StartVertex);

            // Assert

            Assert.Equal(instantSteps, boostSteps, IndexedDfsStepEqualityComparer.Default);
            Assert.Equal(graph.VertexCount, discoveredVertexCount);
            Assert.Equal(expectedStartVertexCount, actualStartVertexCount);

            boostSteps.Dispose();
            instantSteps.Dispose();
            ArrayPool<byte>.Shared.Return(colorMap);
        }
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}
