namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using ColorMap = IndexedDictionary<Color, Color[]>;
    using ColorMapFactory = CachingIndexedDictionaryFactory<Color>;

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

        private Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, List<DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactory,
                ListFactory<IndexedAdjacencyListGraph,
                    DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>>
            Dfs { get; }

        private BaselineDfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>, ColorMap,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactory>
            BaselineDfs { get; }

        private Xunit.Abstractions.ITestOutputHelper Output { get; }

        public DfsTest(Xunit.Abstractions.ITestOutputHelper output)
        {
            Dfs = new Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, List<DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactory,
                ListFactory<IndexedAdjacencyListGraph,
                    DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>>();

            BaselineDfs = new BaselineDfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>, ColorMap,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactory>();

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

            var baselineSteps = BaselineDfs.Traverse(graph, vertex).ToList();
            var boostSteps = Dfs.Traverse(graph, vertex).ToList();

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
        [InlineData(1.059)]
        [InlineData(1.414)]
        [InlineData(1.618)]
        [InlineData(2.0)]
        public void Baseline_and_boost_implementations_should_match_for_forest(double densityPower)
        {
            // Arrange

            IndexedAdjacencyListGraph graph = CreateGraph(densityPower);
            var vertices = new RangeCollection(0, graph.VertexCount);

            // Act

            var baselineSteps = BaselineDfs.Traverse(graph, vertices).ToList();
            var vertexEnumerator = vertices.GetConventionalEnumerator();
            var boostSteps = Dfs.Traverse(graph, vertexEnumerator).ToList();
            var discoveredVertexCount = boostSteps.Count(s => s.Kind == DfsStepKind.DiscoverVertex);
            var expectedStartVertexCount = baselineSteps.Count(s => s.Kind == DfsStepKind.StartVertex);
            var actualStartVertexCount = boostSteps.Count(s => s.Kind == DfsStepKind.StartVertex);

            // Assert

            Assert.Equal(baselineSteps, boostSteps, DfsStepEqualityComparer.Default);
            Assert.Equal(VertexCount, discoveredVertexCount);
            Assert.Equal(expectedStartVertexCount, actualStartVertexCount);
        }

        private IndexedAdjacencyListGraph CreateGraph(double densityPower)
        {
            int edgeCount = (int)Math.Ceiling(Math.Pow(VertexCount, densityPower));

            var builder = new IndexedAdjacencyListGraphBuilder(VertexCount);
            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int source = prng.Next(VertexCount);
                int target = prng.Next(VertexCount);
                builder.Add(SourceTargetPair.Create(source, target));
            }

            var result = builder.MoveToIndexedAdjacencyListGraph();
            return result;
        }
    }
}
