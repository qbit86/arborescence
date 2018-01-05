namespace Ubiquitous
{
    using System.Linq;
    using ColorMap = IndexedDictionary<Color, Color[]>;
    using ColorMapFactoryInstance = IndexedDictionaryFactoryInstance<Color>;

    [BenchmarkDotNet.Attributes.MemoryDiagnoser]
    public abstract class DfsTreeBoostBenchmark
    {
        [BenchmarkDotNet.Attributes.Params(10, 100, 1000)]
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int VertexCount { get; set; }

        private Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>, ColorMap,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactoryInstance>
            Dfs { get; }

        private DfsBaseline<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>, ColorMap,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactoryInstance>
            DfsBaseline { get; }

        private IndexedAdjacencyListGraph Graph { get; set; }

        public DfsTreeBoostBenchmark()
        {
            Dfs = new Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap, IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                ColorMapFactoryInstance>();

            DfsBaseline = new DfsBaseline<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>,
                ColorMap,IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance,
                ColorMapFactoryInstance>();
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
            var steps = Dfs.TraverseBoost(Graph, 0);

            return steps.Count();
        }
    }
}
