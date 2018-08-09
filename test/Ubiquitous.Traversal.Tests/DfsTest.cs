namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Traversal.Advanced;
    using Xunit;
    using Xunit.Abstractions;
    using ColorMap = System.ArraySegment<Traversal.Advanced.Color>;
    using ColorMapConcept = IndexedMapConcept<Traversal.Advanced.Color>;

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
            var colorMapConcept = new ColorMapConcept(VertexCount);

            Dfs = DfsBuilder.WithGraph<IndexedAdjacencyListGraph>()
                .WithVertex<int>().WithEdge<int>()
                .WithEdgeEnumerator<List<int>.Enumerator>()
                .WithColorMap<ColorMap>()
                .WithGraphConcept<IndexedAdjacencyListGraphInstance>()
                .WithColorMapConcept(colorMapConcept)
                .Create();

            BaselineDfs = BaselineDfsBuilder.WithGraph<IndexedAdjacencyListGraph>()
                .WithVertex<int>().WithEdge<int>()
                .WithEdgeEnumerator<List<int>.Enumerator>()
                .WithColorMap<ColorMap>()
                .WithGraphConcept<IndexedAdjacencyListGraphInstance>()
                .WithColorMapConcept(colorMapConcept)
                .Create();

            Output = output;
        }

        private Dfs<IndexedAdjacencyListGraph, int, int, List<int>.Enumerator, ColorMap,
                IndexedAdjacencyListGraphInstance, ColorMapConcept>
            Dfs { get; }

        private BaselineDfs<IndexedAdjacencyListGraph, int, int, List<int>.Enumerator, ColorMap,
                IndexedAdjacencyListGraphInstance, ColorMapConcept>
            BaselineDfs { get; }

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

            IndexedAdjacencyListGraph graph = CreateGraph(densityPower);
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

            IndexedAdjacencyListGraph graph = CreateGraph(densityPower);
            var vertices = new RangeCollection(0, graph.VertexCount);

            // Act

            RangeCollection.Enumerator vertexEnumerator = vertices.GetConventionalEnumerator();
            List<Step<DfsStepKind, int, int>> baselineSteps = BaselineDfs.Traverse(graph, vertexEnumerator).ToList();
            List<Step<DfsStepKind, int, int>> boostSteps = Dfs.Traverse(graph, vertexEnumerator).ToList();
            int discoveredVertexCount = boostSteps.Count(s => s.Kind == DfsStepKind.DiscoverVertex);
            int expectedStartVertexCount = baselineSteps.Count(s => s.Kind == DfsStepKind.StartVertex);
            int actualStartVertexCount = boostSteps.Count(s => s.Kind == DfsStepKind.StartVertex);

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

            IndexedAdjacencyListGraph result = builder.MoveToIndexedAdjacencyListGraph();
            return result;
        }
    }
}
