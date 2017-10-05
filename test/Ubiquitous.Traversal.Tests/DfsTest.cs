namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using ColorMap = IndexedDictionary<Color, Color[]>;
    using ColorMapFactoryInstance = IndexedDictionaryFactoryInstance<Color>;

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
        private Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>, ColorMap,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactoryInstance>
            Dfs { get; }

        private Xunit.Abstractions.ITestOutputHelper Output { get; }

        public DfsTest(Xunit.Abstractions.ITestOutputHelper output)
        {
            Dfs = new Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>, ColorMap,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactoryInstance>();

            Output = output;
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(1.414)]
        [InlineData(1.5)]
        [InlineData(1.618)]
        [InlineData(2.0)]
        public void Baseline_and_boost_implementations_should_match_for_tree(double densityPower)
        {
            // Arrange

            IndexedAdjacencyListGraph graph = CreateGraph(densityPower);
            int vertex = 0;

            // Act

            var baselineSteps = Dfs.TraverseBaseline(graph, vertex).ToList();
            var boostSteps = Dfs.TraverseBoost(graph, vertex).ToList();

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
                var baselineStep = baselineSteps[i];
                var boostStep = boostSteps[i];

                if (baselineStep == boostStep)
                    continue;

                Output.WriteLine($"{nameof(i)}: {i}, "
                    + $"{nameof(baselineStep)}: {baselineStep}, {nameof(boostStep)}: {boostStep}");
            }

            Assert.Equal(baselineSteps, boostSteps, DfsStepEqualityComparer.Default);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(1.414)]
        [InlineData(1.5)]
        [InlineData(1.618)]
        [InlineData(2.0)]
        public void Baseline_and_boost_implementations_should_match_for_forest(double densityPower)
        {
            // Arrange

            IndexedAdjacencyListGraph graph = CreateGraph(densityPower);
            var vertices = new RangeCollection(0, graph.VertexCount);

            // Act

            var baselineSteps = Dfs.TraverseBaseline(graph, vertices).ToList();
            var boostSteps = Dfs.TraverseBoost(graph, vertices).ToList();

            // Assert

            Assert.Equal(baselineSteps, boostSteps, DfsStepEqualityComparer.Default);
        }

        private IndexedAdjacencyListGraph CreateGraph(double densityPower)
        {
            const int vertexCount = 100;
            int edgeCount = (int)Math.Ceiling(Math.Pow(vertexCount, densityPower));

            var builder = new IndexedAdjacencyListGraphBuilder(vertexCount);
            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int source = prng.Next(vertexCount);
                int target = prng.Next(vertexCount);
                builder.Add(SourceTargetPair.Create(source, target));
            }

            var result = builder.MoveToIndexedAdjacencyListGraph();
            return result;
        }
    }
}
