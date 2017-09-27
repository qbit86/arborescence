namespace Ubiquitous
{
    using System;
    using System.Linq;
    using ColorMap = IndexedDictionary<Color, Color[]>;
    using ColorMapFactoryInstance = IndexedDictionaryFactoryInstance<Color>;

    [BenchmarkDotNet.Attributes.MemoryDiagnoser]
    public abstract class DfsTreeBaselineVsRecursiveBenchmark
    {
        [BenchmarkDotNet.Attributes.Params(10, 100, 1000)]
        public int VertexCount { get; set; }

        private Dfs<IndexedAdjacencyListGraph, int, int, ImmutableArrayEnumeratorAdapter<int>, ColorMap,
            IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance, ColorMapFactoryInstance> Dfs { get; }

        private IndexedAdjacencyListGraph Graph { get; set; }

        public DfsTreeBaselineVsRecursiveBenchmark()
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
        public int DfsTreeBaseline()
        {
            var steps = Dfs.TraverseBaseline(Graph, 0);

            return steps.Count();
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public int DfsTreeRecursive()
        {
            var steps = Dfs.TraverseRecursively(Graph, 0);

            return steps.Count();
        }
    }
}
