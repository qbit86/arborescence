namespace Ubiquitous
{
    using System.Linq;
    using ColorMap = IndexedDictionary<Color, Color[]>;
    using ColorMapFactoryInstance = IndexedDictionaryFactoryInstance<Color>;

    [BenchmarkDotNet.Attributes.MemoryDiagnoser]
    public abstract class DfsForestBaselineVsNonRecursiveBenchmark
    {
        [BenchmarkDotNet.Attributes.Params(10, 100, 1000)]
        public int VertexCount { get; set; }

        private Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>, ColorMap,
            IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactoryInstance> Dfs { get; }

        private IndexedAdjacencyListGraph Graph { get; set; }

        public DfsForestBaselineVsNonRecursiveBenchmark()
        {
            Dfs = new Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>, ColorMap,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactoryInstance>();
        }

        [BenchmarkDotNet.Attributes.GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);
        }

        [BenchmarkDotNet.Attributes.Benchmark(Baseline = true)]
        public int DfsForestBaseline()
        {
            var steps = Dfs.TraverseBaseline(
                Graph, new RangeCollection(0, Graph.VertexCount));

            return steps.Count();
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public int DfsForestNonRecursive()
        {
            var steps = Dfs.TraverseNonRecursively(
                Graph, new RangeCollection(0, Graph.VertexCount));

            return steps.Count();
        }
    }
}
