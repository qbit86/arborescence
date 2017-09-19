namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ColorMap = IndexedDictionary<Color, Color[]>;
    using ColorMapFactoryInstance = IndexedDictionaryFactoryInstance<Color>;

    [BenchmarkDotNet.Attributes.MemoryDiagnoser]
    public abstract class DfsBenchmark
    {
        [BenchmarkDotNet.Attributes.Params(10, 100, 1000)]
        public int VertexCount { get; set; }

        private Dfs<IndexedAdjacencyListGraph, int, int, IEnumerable<int>,
            IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance> Dfs { get; set; }

        private IndexedAdjacencyListGraph Graph { get; set; }

        public DfsBenchmark()
        {
            Dfs = new Dfs<IndexedAdjacencyListGraph, int, int, IEnumerable<int>,
                IndexedAdjacencyListGraphInstance, IndexedAdjacencyListGraphInstance>();
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
        public int RecursiveDfs()
        {
            var steps = Dfs.TraverseRecursively<RangeCollection, ColorMap, ColorMapFactoryInstance>(
                Graph, new RangeCollection(0, Graph.VertexCount));

            return steps.Count();
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public int NonRecursiveDfs()
        {
            var steps = Dfs.TraverseNonRecursively<RangeCollection, ColorMap, ColorMapFactoryInstance>(
                Graph, new RangeCollection(0, Graph.VertexCount));

            return steps.Count();
        }
    }
}
