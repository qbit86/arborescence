namespace Ubiquitous
{
    using ColorMap = IndexedDictionary<Color, Color[]>;
    using Stack = System.Collections.Generic.List<DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>;
    using ColorMapFactory = IndexedDictionaryFactory<Color>;
    using CachingColorMapFactory = CachingIndexedDictionaryFactory<Color>;
    using ListFactory = ListFactory<IndexedAdjacencyListGraph,
        DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>;
    using CachingListFactory = CachingListFactory<IndexedAdjacencyListGraph,
        DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>;

    [BenchmarkDotNet.Attributes.MemoryDiagnoser]
    public abstract class DfsTreeBoostBenchmark
    {
        [BenchmarkDotNet.Attributes.Params(10, 100, 1000)]
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int VertexCount { get; set; }

        private BaselineDfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>, ColorMap,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactory>
            BaselineDfs { get; }

        private Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, Stack,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                ColorMapFactory, ListFactory>
            DefaultDfs { get; }

        private Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, Stack,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                CachingColorMapFactory, CachingListFactory>
            CachingDfs { get; set; }

        private IndexedAdjacencyListGraph Graph { get; set; }

        protected DfsTreeBoostBenchmark()
        {
            BaselineDfs = BaselineDfsFactory.WithGraph<IndexedAdjacencyListGraph>()
                .WithVertex<int>().WithEdge<int>()
                .WithEdgeEnumerator<ImmutableArrayEnumeratorAdapter<int>>()
                .WithColorMap<ColorMap>()
                .WithVertexConcept<IndexedAdjacencyListGraphInstance>()
                .WithEdgeConcept<IndexedAdjacencyListGraphInstance>()
                .WithColorMapFactory<ColorMapFactory>()
                .Create();

            DefaultDfs = new Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, Stack,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                ColorMapFactory, ListFactory>();
        }

        [BenchmarkDotNet.Attributes.GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            var colorMapFactory = new CachingColorMapFactory();
            colorMapFactory.Warmup(VertexCount);
            var stackFactory = new CachingListFactory(VertexCount);
            stackFactory.Warmup();

            CachingDfs = new Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, Stack,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                CachingColorMapFactory, CachingListFactory>(colorMapFactory, stackFactory);
        }

        [BenchmarkDotNet.Attributes.Benchmark(Baseline = true)]
        public int BaselineDfsTree()
        {
            int count = 0;
            var steps = BaselineDfs.Traverse(Graph, 0);
            foreach (var unused in steps)
            {
                ++count;
            }

            return count;
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public int DefaultDfsTree()
        {
            int count = 0;
            var steps = DefaultDfs.Traverse(Graph, 0);
            foreach (var unused in steps)
            {
                ++count;
            }

            return count;
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public int CachingDfsTree()
        {
            int count = 0;
            var steps = CachingDfs.Traverse(Graph, 0);
            foreach (var unused in steps)
            {
                ++count;
            }

            return count;
        }
    }
}
