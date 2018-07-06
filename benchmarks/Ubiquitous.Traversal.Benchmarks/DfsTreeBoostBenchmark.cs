// ReSharper disable SuggestVarOrType_Elsewhere

namespace Ubiquitous
{
    using System.Collections.Generic;
    using BenchmarkDotNet.Attributes;
    using Traversal.Advanced;
    using ColorMap = IndexedDictionary<Traversal.Advanced.Color, Traversal.Advanced.Color[]>;
    using ColorMapFactory = IndexedDictionaryFactory<Traversal.Advanced.Color>;
    using CachingColorMapFactory = CachingIndexedDictionaryFactory<Traversal.Advanced.Color>;
    using ListFactory =
        ListFactory<IndexedAdjacencyListGraph,
            Traversal.Advanced.DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>;

    [MemoryDiagnoser]
    public abstract class DfsTreeBoostBenchmark
    {
        protected DfsTreeBoostBenchmark()
        {
            BaselineDfs = BaselineDfsBuilder.WithGraph<IndexedAdjacencyListGraph>()
                .WithVertex<int>().WithEdge<int>()
                .WithEdgeEnumerator<ImmutableArrayEnumeratorAdapter<int>>()
                .WithColorMap<ColorMap>()
                .WithGraphConcept<IndexedAdjacencyListGraphInstance>()
                .WithColorMapFactory<ColorMapFactory>()
                .Create();

            DefaultDfs = DfsBuilder.WithGraph<IndexedAdjacencyListGraph>()
                .WithVertex<int>().WithEdge<int>()
                .WithEdgeEnumerator<ImmutableArrayEnumeratorAdapter<int>>()
                .WithColorMap<ColorMap>()
                .WithGraphConcept<IndexedAdjacencyListGraphInstance>()
                .WithColorMapFactory<ColorMapFactory>()
                .Create();
        }

        [Params(10, 100, 1000)]
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int VertexCount { get; set; }

        private BaselineDfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>, ColorMap,
                IndexedAdjacencyListGraphInstance, ColorMapFactory>
            BaselineDfs { get; }

        private Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap,
                IndexedAdjacencyListGraphInstance, ColorMapFactory>
            DefaultDfs { get; }

        private Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap,
                IndexedAdjacencyListGraphInstance, CachingColorMapFactory>
            CachingDfs { get; set; }

        private IndexedAdjacencyListGraph Graph { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            var colorMapFactory = new CachingColorMapFactory();
            colorMapFactory.Warmup(VertexCount);

            CachingDfs = DfsBuilder.WithGraph<IndexedAdjacencyListGraph>()
                .WithVertex<int>().WithEdge<int>()
                .WithEdgeEnumerator<ImmutableArrayEnumeratorAdapter<int>>()
                .WithColorMap<ColorMap>()
                .WithGraphConcept<IndexedAdjacencyListGraphInstance>()
                .WithColorMapFactory(colorMapFactory)
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
