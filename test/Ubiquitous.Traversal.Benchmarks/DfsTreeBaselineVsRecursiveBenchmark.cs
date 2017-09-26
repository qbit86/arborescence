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
            int vertexCount = VertexCount;
            int edgeCount = (int)Math.Ceiling(Math.Pow(VertexCount, 1.618));

            var builder = new IndexedAdjacencyListGraphBuilder(vertexCount);
            var prng = new Random(1729);

            for (int e = 0; e < edgeCount; ++e)
            {
                int source = prng.Next(vertexCount);
                int target = prng.Next(vertexCount);
                builder.Add(SourceTargetPair.Create(source, target));
            }

            Graph = builder.MoveToIndexedAdjacencyListGraph();
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
