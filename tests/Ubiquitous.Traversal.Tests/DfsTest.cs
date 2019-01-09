namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Misnomer;
    using Models;
    using Traversal;
    using Traversal.Advanced;
    using Xunit;
    using Xunit.Abstractions;
    using ColorMap = ArrayPrefix<Traversal.Color>;
    using ColorMapPolicy = Models.IndexedMapPolicy<Traversal.Color>;
    using EdgeEnumerator = ArraySegmentEnumerator<int>;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    internal sealed class DfsStepEqualityComparer : IEqualityComparer<Step<DfsStepKind, int, int>>
    {
        internal static DfsStepEqualityComparer Default { get; } = new DfsStepEqualityComparer();

        public bool Equals(Step<DfsStepKind, int, int> x, Step<DfsStepKind, int, int> y)
        {
            if (x.Kind != y.Kind)
                return false;

            if (x.Vertex != y.Vertex)
                return false;

            if (x.Edge != y.Edge)
                return false;

            return true;
        }

        public int GetHashCode(Step<DfsStepKind, int, int> obj)
        {
            return ((int)obj.Kind).GetHashCode() ^ obj.Vertex.GetHashCode() ^ obj.Edge.GetHashCode();
        }
    }

    public sealed class DfsTest
    {
        private const int VertexCount = 100;

        public DfsTest(ITestOutputHelper output)
        {
            var colorMapPolicy = new ColorMapPolicy(VertexCount);

            Dfs = Dfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, ColorMap, IndexedDfsStep>
                .Create(default(IndexedAdjacencyListGraphPolicy), colorMapPolicy, default(IndexedDfsStepPolicy));

            MultipleSourceDfs = MultipleSourceDfs<AdjacencyListIncidenceGraph, int, int, IndexCollection,
                IndexCollectionEnumerator, EdgeEnumerator, ColorMap, IndexedDfsStep>.Create(
                default(IndexedAdjacencyListGraphPolicy), colorMapPolicy, default(IndexCollectionEnumerablePolicy),
                default(IndexedDfsStepPolicy));

            BaselineDfs = BaselineDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, ColorMap, IndexedDfsStep>
                .Create(default(IndexedAdjacencyListGraphPolicy), colorMapPolicy, default(IndexedDfsStepPolicy));

            BaselineMultipleSourceDfs = BaselineMultipleSourceDfs<AdjacencyListIncidenceGraph, int, int,
                IndexCollection, IndexCollectionEnumerator, EdgeEnumerator, ColorMap>.Create(
                default(IndexedAdjacencyListGraphPolicy), colorMapPolicy, default(IndexCollectionEnumerablePolicy));

            Output = output;
        }

        private Dfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, ColorMap, IndexedDfsStep,
                IndexedAdjacencyListGraphPolicy, ColorMapPolicy, IndexedDfsStepPolicy>
            Dfs { get; }

        private MultipleSourceDfs<AdjacencyListIncidenceGraph, int, int,
                IndexCollection, IndexCollectionEnumerator, EdgeEnumerator,
                ColorMap, IndexedDfsStep,
                IndexedAdjacencyListGraphPolicy, ColorMapPolicy,
                IndexCollectionEnumerablePolicy, IndexedDfsStepPolicy>
            MultipleSourceDfs { get; }

        private BaselineDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, ColorMap,
                IndexedDfsStep,
                IndexedAdjacencyListGraphPolicy, ColorMapPolicy, IndexedDfsStepPolicy>
            BaselineDfs { get; }

        private BaselineMultipleSourceDfs<AdjacencyListIncidenceGraph, int, int,
                IndexCollection, IndexCollectionEnumerator, EdgeEnumerator, ColorMap,
                Step<DfsStepKind, int, int>,
                IndexedAdjacencyListGraphPolicy, ColorMapPolicy,
                IndexCollectionEnumerablePolicy, StepPolicy<DfsStepKind, int, int>>
            BaselineMultipleSourceDfs { get; }

        private ITestOutputHelper Output { get; }

        [Theory]
        [InlineData(1.0)]
        [InlineData(1.414)]
        [InlineData(1.5)]
        [InlineData(1.618)]
        [InlineData(2.0)]
        public void Baseline_and_boost_implementations_should_match_for_tree(double densityPower)
        {
            throw new NotImplementedException();
            /*
            // Arrange

            AdjacencyListIncidenceGraph graph = CreateGraph(densityPower);
            int vertex = 0;
            int stepCountApproximation = graph.VertexCount + graph.EdgeCount;

            // Act

            Rist<Step<DfsStepKind, int, int>> baselineSteps = RistFactory<Step<DfsStepKind, int, int>>.Create(
                BaselineDfs.Traverse(graph, vertex).GetEnumerator(), stepCountApproximation);
            Rist<IndexedDfsStep> boostSteps = RistFactory<IndexedDfsStep>.Create(
                Dfs.Traverse(graph, vertex).GetEnumerator(), stepCountApproximation);

            // Assert

            int baselineStepCount = baselineSteps.Count;
            int boostStepCount = boostSteps.Count;
            if (baselineStepCount != boostStepCount)
            {
                Output.WriteLine($"{nameof(baselineStepCount)}: {baselineStepCount}, "
                    + $"{nameof(boostStepCount)}: {boostStepCount}");
            }

            int count = Math.Min(baselineStepCount, boostStepCount);
            for (int i = 0; i != count; ++i)
            {
                Step<DfsStepKind, int, int> baselineStep = baselineSteps[i];
                IndexedDfsStep boostStep = boostSteps[i];

                if (baselineStep == boostStep)
                    continue;

                Output.WriteLine($"{nameof(i)}: {i}, "
                    + $"{nameof(baselineStep)}: {baselineStep}, {nameof(boostStep)}: {boostStep}");
            }

            Assert.Equal(baselineSteps, boostSteps, DfsStepEqualityComparer.Default);

            baselineSteps.Dispose();
            boostSteps.Dispose();
            */
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(1.059)]
        [InlineData(1.414)]
        [InlineData(1.618)]
        [InlineData(2.0)]
        public void Baseline_and_boost_implementations_should_match_for_forest(double densityPower)
        {
            throw new NotImplementedException();
            /*
            // Arrange

            AdjacencyListIncidenceGraph graph = CreateGraph(densityPower);
            var vertices = new IndexCollection(graph.VertexCount);
            int stepCountApproximation = graph.VertexCount + graph.EdgeCount;

            // Act

            Rist<Step<DfsStepKind, int, int>> baselineSteps = RistFactory<Step<DfsStepKind, int, int>>.Create(
                BaselineMultipleSourceDfs.Traverse(graph, vertices).GetEnumerator(), stepCountApproximation);
            Rist<Step<DfsStepKind, int, int>> boostSteps = RistFactory<Step<DfsStepKind, int, int>>.Create(
                MultipleSourceDfs.Traverse(graph, vertices).GetEnumerator(), stepCountApproximation);
            int discoveredVertexCount = boostSteps.Count(s => s.Kind == DfsStepKind.DiscoverVertex);
            int expectedStartVertexCount = baselineSteps.Count(s => s.Kind == DfsStepKind.StartVertex);
            int actualStartVertexCount = boostSteps.Count(s => s.Kind == DfsStepKind.StartVertex);

            // Assert

            Assert.Equal(baselineSteps, boostSteps, DfsStepEqualityComparer.Default);
            Assert.Equal(VertexCount, discoveredVertexCount);
            Assert.Equal(expectedStartVertexCount, actualStartVertexCount);

            baselineSteps.Dispose();
            boostSteps.Dispose();
            */
        }

        private AdjacencyListIncidenceGraph CreateGraph(double densityPower)
        {
            int edgeCount = (int)Math.Ceiling(Math.Pow(VertexCount, densityPower));

            var builder = new AdjacencyListIncidenceGraphBuilder(VertexCount);
            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int source = prng.Next(VertexCount);
                int target = prng.Next(VertexCount);
                builder.TryAdd(source, target, out _);
            }

            AdjacencyListIncidenceGraph result = builder.ToGraph();
            return result;
        }
    }
}
