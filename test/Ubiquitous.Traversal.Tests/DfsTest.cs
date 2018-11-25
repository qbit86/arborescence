namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Traversal.Advanced;
    using Xunit;
    using Xunit.Abstractions;
    using ColorMap = ArrayPrefix<Traversal.Color>;
    using ColorMapPolicy = IndexedMapPolicy<Traversal.Color>;
    using IndexedAdjacencyListGraphPolicy =
        IndexedIncidenceGraphPolicy<JaggedAdjacencyListIncidenceGraph, ArrayPrefixEnumerator<int>>;

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
            return obj.Kind.GetHashCode() ^ obj.Vertex.GetHashCode() ^ obj.Edge.GetHashCode();
        }
    }

    public class DfsTest
    {
        private const int VertexCount = 100;

        public DfsTest(ITestOutputHelper output)
        {
            var colorMapPolicy = new ColorMapPolicy(VertexCount);

            Dfs = new Dfs<JaggedAdjacencyListIncidenceGraph, int, int,
                ArrayPrefixEnumerator<int>, ColorMap, IndexedAdjacencyListGraphPolicy, ColorMapPolicy>(
                default(IndexedAdjacencyListGraphPolicy), colorMapPolicy);

            MultipleSourceDfs = new MultipleSourceDfs<JaggedAdjacencyListIncidenceGraph, int, int,
                IndexCollection, IndexCollectionEnumerator, ArrayPrefixEnumerator<int>,
                ColorMap, IndexedAdjacencyListGraphPolicy, ColorMapPolicy, IndexCollectionEnumerablePolicy>(
                default(IndexedAdjacencyListGraphPolicy), colorMapPolicy, default(IndexCollectionEnumerablePolicy));

            BaselineDfs = new BaselineDfs<JaggedAdjacencyListIncidenceGraph, int, int,
                ArrayPrefixEnumerator<int>, ColorMap, IndexedAdjacencyListGraphPolicy, ColorMapPolicy>(
                default(IndexedAdjacencyListGraphPolicy), colorMapPolicy);

            BaselineMultipleSourceDfs = new BaselineMultipleSourceDfs<JaggedAdjacencyListIncidenceGraph, int, int,
                IndexCollection, IndexCollectionEnumerator, ArrayPrefixEnumerator<int>,
                ColorMap, IndexedAdjacencyListGraphPolicy, ColorMapPolicy, IndexCollectionEnumerablePolicy>(
                default(IndexedAdjacencyListGraphPolicy), colorMapPolicy, default(IndexCollectionEnumerablePolicy));

            Output = output;
        }

        private Dfs<JaggedAdjacencyListIncidenceGraph, int, int, ArrayPrefixEnumerator<int>, ColorMap,
                IndexedAdjacencyListGraphPolicy, ColorMapPolicy>
            Dfs { get; }

        private MultipleSourceDfs<JaggedAdjacencyListIncidenceGraph, int, int,
                IndexCollection, IndexCollectionEnumerator, ArrayPrefixEnumerator<int>,
                ColorMap, IndexedAdjacencyListGraphPolicy, ColorMapPolicy, IndexCollectionEnumerablePolicy>
            MultipleSourceDfs { get; }

        private BaselineDfs<JaggedAdjacencyListIncidenceGraph, int, int, ArrayPrefixEnumerator<int>, ColorMap,
                IndexedAdjacencyListGraphPolicy, ColorMapPolicy>
            BaselineDfs { get; }

        private BaselineMultipleSourceDfs<JaggedAdjacencyListIncidenceGraph, int, int,
                IndexCollection, IndexCollectionEnumerator, ArrayPrefixEnumerator<int>, ColorMap,
                IndexedAdjacencyListGraphPolicy, ColorMapPolicy, IndexCollectionEnumerablePolicy>
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
            // Arrange

            JaggedAdjacencyListIncidenceGraph graph = CreateGraph(densityPower);
            int vertex = 0;

            // Act

            List<Step<DfsStepKind, int, int>> baselineSteps = BaselineDfs.Traverse(graph, vertex).ToList();
            List<Step<DfsStepKind, int, int>> boostSteps = Dfs.Traverse(graph, vertex).ToList();

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
                Step<DfsStepKind, int, int> boostStep = boostSteps[i];

                if (baselineStep == boostStep)
                    continue;

                Output.WriteLine($"{nameof(i)}: {i}, "
                    + $"{nameof(baselineStep)}: {baselineStep}, {nameof(boostStep)}: {boostStep}");
            }

            Assert.Equal(baselineSteps, boostSteps, DfsStepEqualityComparer.Default);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(1.059)]
        [InlineData(1.414)]
        [InlineData(1.618)]
        [InlineData(2.0)]
        public void Baseline_and_boost_implementations_should_match_for_forest(double densityPower)
        {
            // Arrange

            JaggedAdjacencyListIncidenceGraph graph = CreateGraph(densityPower);
            var vertices = new IndexCollection(graph.VertexUpperBound);

            // Act

            List<Step<DfsStepKind, int, int>> baselineSteps =
                BaselineMultipleSourceDfs.Traverse(graph, vertices).ToList();
            List<Step<DfsStepKind, int, int>> boostSteps = MultipleSourceDfs.Traverse(graph, vertices).ToList();
            int discoveredVertexCount = boostSteps.Count(s => s.Kind == DfsStepKind.DiscoverVertex);
            int expectedStartVertexCount = baselineSteps.Count(s => s.Kind == DfsStepKind.StartVertex);
            int actualStartVertexCount = boostSteps.Count(s => s.Kind == DfsStepKind.StartVertex);

            // Assert

            Assert.Equal(baselineSteps, boostSteps, DfsStepEqualityComparer.Default);
            Assert.Equal(VertexCount, discoveredVertexCount);
            Assert.Equal(expectedStartVertexCount, actualStartVertexCount);
        }

        private JaggedAdjacencyListIncidenceGraph CreateGraph(double densityPower)
        {
            int edgeCount = (int)Math.Ceiling(Math.Pow(VertexCount, densityPower));

            var builder = new JaggedAdjacencyListIncidenceGraphBuilder(VertexCount);
            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int source = prng.Next(VertexCount);
                int target = prng.Next(VertexCount);
                builder.TryAdd(source, target, out _);
            }

            JaggedAdjacencyListIncidenceGraph result = builder.MoveToIndexedAdjacencyListGraph();
            return result;
        }
    }
}
