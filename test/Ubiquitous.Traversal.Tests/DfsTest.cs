namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using ColorMap = IndexedDictionary<Color, Color[]>;

    internal struct ColorMapFactoryInstance : IFactoryConcept<IndexedAdjacencyListGraph, ColorMap>
    {
        public ColorMap Acquire(IndexedAdjacencyListGraph graph)
        {
            return IndexedDictionary.Create(new Color[graph.VertexCount]);
        }

        public void Release(IndexedAdjacencyListGraph graph, ColorMap value)
        {
        }
    }

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
        [Theory]
        [InlineData(1.0)]
        [InlineData(1.414)]
        [InlineData(1.5)]
        [InlineData(1.618)]
        [InlineData(2.0)]
        public void Recursive_and_non_recursive_implementations_should_match(double densityPower)
        {
            // Arrange

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

            IndexedAdjacencyListGraph graph = builder.MoveToIndexedAdjacencyListGraph();

            var dfs = new Dfs<IndexedAdjacencyListGraph, int, int, IEnumerable<int>,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance>();

            // Act

            var recursiveSteps = dfs.TraverseRecursively<IEnumerable<int>, ColorMap, ColorMapFactoryInstance>(
                graph, Enumerable.Range(0, graph.VertexCount)).ToList();

            var nonRecursiveSteps = dfs.TraverseNonRecursively<IEnumerable<int>, ColorMap, ColorMapFactoryInstance>(
                graph, Enumerable.Range(0, graph.VertexCount)).ToList();

            // Assert

            Assert.Equal(recursiveSteps, nonRecursiveSteps, DfsStepEqualityComparer.Default);
        }
    }
}
