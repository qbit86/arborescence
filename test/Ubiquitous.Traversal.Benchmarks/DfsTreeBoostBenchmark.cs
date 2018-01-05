namespace Ubiquitous
{
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

        private Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>, ColorMap,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactory>
            Dfs { get; }

        private DfsBaseline<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>, ColorMap,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactory>
            DfsBaseline { get; }

        private IndexedAdjacencyListGraph Graph { get; set; }

        public DfsTreeBoostBenchmark()
        {
            Dfs = new Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                ColorMapFactory>();

            DfsBaseline = new DfsBaseline<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                ColorMapFactory>();
        }

        [BenchmarkDotNet.Attributes.GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);
        }

        [BenchmarkDotNet.Attributes.Benchmark(Baseline = true)]
        public int DfsTreeBaseline()
        {
            var steps = DfsBaseline.Traverse(Graph, 0);

            return steps.Count();
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public int DfsTreeBoost()
        {
            var steps = Dfs.Traverse(Graph, 0);

            return steps.Count();
        }
    }
}
