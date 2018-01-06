namespace Ubiquitous
{
    using System.Linq;
    using ColorMap = IndexedDictionary<Color, Color[]>;
    using Stack = System.Collections.Generic.List<DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>;
    using ColorMapFactory = IndexedDictionaryFactory<Color>;
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
                ColorMapFactory, CachingListFactory>
            CachingDfs { get; set; }

        private IndexedAdjacencyListGraph Graph { get; set; }

        public DfsTreeBoostBenchmark()
        {
            BaselineDfs = new BaselineDfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                ColorMapFactory>();

            DefaultDfs = new Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, Stack,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                ColorMapFactory, ListFactory>();
        }

        [BenchmarkDotNet.Attributes.GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            var colorMapFactory = default(ColorMapFactory);
            var stackFactory = new CachingListFactory(VertexCount);
            stackFactory.Warmup();

            CachingDfs = new Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, Stack,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                ColorMapFactory, CachingListFactory>(colorMapFactory, stackFactory);
        }

        [BenchmarkDotNet.Attributes.Benchmark(Baseline = true)]
        public int BaselineDfsTree()
        {
            var steps = BaselineDfs.Traverse(Graph, 0);

            return steps.Count();
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public int DefaultDfsTree()
        {
            var steps = DefaultDfs.Traverse(Graph, 0);

            return steps.Count();
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public int CachingDfsTree()
        {
            var steps = CachingDfs.Traverse(Graph, 0);

            return steps.Count();
        }
    }
}
