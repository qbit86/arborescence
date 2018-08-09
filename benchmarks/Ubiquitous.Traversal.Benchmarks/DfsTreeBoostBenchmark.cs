// ReSharper disable SuggestVarOrType_Elsewhere

namespace Ubiquitous
{
    using System.Collections.Generic;
    using BenchmarkDotNet.Attributes;
    using Traversal.Advanced;
    using ColorMap = System.ArraySegment<Traversal.Advanced.Color>;
    using ColorMapConcept = IndexedMapConcept<Traversal.Advanced.Color>;

    [MemoryDiagnoser]
    public abstract class DfsTreeBoostBenchmark
    {
        protected DfsTreeBoostBenchmark()
        {
            var colorMapConcept = new ColorMapConcept(VertexCount);

            BaselineDfs = BaselineDfsBuilder.WithGraph<IndexedAdjacencyListGraph>()
                .WithVertex<int>().WithEdge<int>()
                .WithEdgeEnumerator<List<int>.Enumerator>()
                .WithColorMap<ColorMap>()
                .WithGraphConcept<IndexedAdjacencyListGraphConcept>()
                .WithColorMapConcept(colorMapConcept)
                .Create();

            DefaultDfs = DfsBuilder.WithGraph<IndexedAdjacencyListGraph>()
                .WithVertex<int>().WithEdge<int>()
                .WithEdgeEnumerator<List<int>.Enumerator>()
                .WithColorMap<ColorMap>()
                .WithGraphConcept<IndexedAdjacencyListGraphConcept>()
                .WithColorMapConcept(colorMapConcept)
                .Create();
        }

        [Params(10, 100, 1000)]
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int VertexCount { get; set; }

        private BaselineDfs<IndexedAdjacencyListGraph, int, int, List<int>.Enumerator, ColorMap,
                IndexedAdjacencyListGraphConcept, ColorMapConcept>
            BaselineDfs { get; }

        private Dfs<IndexedAdjacencyListGraph, int, int, List<int>.Enumerator, ColorMap,
                IndexedAdjacencyListGraphConcept, ColorMapConcept>
            DefaultDfs { get; }

        private Dfs<IndexedAdjacencyListGraph, int, int, List<int>.Enumerator, ColorMap,
                IndexedAdjacencyListGraphConcept, ColorMapConcept>
            CachingDfs { get; set; }

        private IndexedAdjacencyListGraph Graph { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            var indexedMapConcept = new ColorMapConcept(VertexCount);
            indexedMapConcept.Warmup();

            CachingDfs = DfsBuilder.WithGraph<IndexedAdjacencyListGraph>()
                .WithVertex<int>().WithEdge<int>()
                .WithEdgeEnumerator<List<int>.Enumerator>()
                .WithColorMap<ColorMap>()
                .WithGraphConcept<IndexedAdjacencyListGraphConcept>()
                .WithColorMapConcept(indexedMapConcept)
                .Create();
        }

        [Benchmark(Baseline = true)]
        public int BaselineDfsTree()
        {
            int count = 0;
            IEnumerable<Step<DfsStepKind, int, int>> steps = BaselineDfs.Traverse(Graph, 0);
            foreach (Step<DfsStepKind, int, int> _ in steps)
                ++count;

            return count;
        }

        [Benchmark]
        public int DefaultDfsTree()
        {
            int count = 0;
            var steps = DefaultDfs.Traverse(Graph, 0);
            foreach (Step<DfsStepKind, int, int> _ in steps)
                ++count;

            return count;
        }

        [Benchmark]
        public int CachingDfsTree()
        {
            int count = 0;
            var steps = CachingDfs.Traverse(Graph, 0);
            foreach (Step<DfsStepKind, int, int> _ in steps)
                ++count;

            return count;
        }
    }
}
