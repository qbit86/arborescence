namespace Ubiquitous
{
    using System.Collections.Generic;
    using System.Linq;
    using ColorMap = IndexedDictionary<Color, Color[]>;
    using ColorMapFactory = IndexedDictionaryFactory<Color>;

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
                ColorMap, List<DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactory,
                ListFactory<IndexedAdjacencyListGraph,
                    DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>>
            DefaultDfs { get; }

        private Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, List<DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactory,
                CachingListFactory<IndexedAdjacencyListGraph,
                    DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>>
            CachingDfs { get; set; }

        private IndexedAdjacencyListGraph Graph { get; set; }

        public DfsTreeBoostBenchmark()
        {
            BaselineDfs = new BaselineDfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                ColorMapFactory>();

            DefaultDfs = new Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, List<DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                ColorMapFactory,
                ListFactory<IndexedAdjacencyListGraph,
                    DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>>();
        }

        [BenchmarkDotNet.Attributes.GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            var colorMapFactory = default(ColorMapFactory);
            var stackFactory = new CachingListFactory<IndexedAdjacencyListGraph,
                DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>(VertexCount);
            stackFactory.Warmup();

            CachingDfs = new Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, List<DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                ColorMapFactory,
                CachingListFactory<IndexedAdjacencyListGraph,
                    DfsStackFrame<int, int, ImmutableArrayEnumeratorAdapter<int>>>>(colorMapFactory, stackFactory);
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
