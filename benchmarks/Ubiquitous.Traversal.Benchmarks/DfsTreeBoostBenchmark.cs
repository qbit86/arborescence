// ReSharper disable SuggestVarOrType_Elsewhere

namespace Ubiquitous
{
    using System.Collections.Generic;
    using BenchmarkDotNet.Attributes;
    using Traversal.Advanced;
    using ColorMap = System.ArraySegment<Traversal.Advanced.Color>;
    using ColorMapConcept = IndexedMapPolicy<Traversal.Advanced.Color>;

    [MemoryDiagnoser]
    public abstract class DfsTreeBoostBenchmark
    {
        protected DfsTreeBoostBenchmark()
        {
            var colorMapConcept = new ColorMapConcept(VertexCount);

            BaselineDfs = new BaselineDfs<IndexedAdjacencyListGraph, int, int,
                List<int>.Enumerator, ColorMap, IndexedAdjacencyListGraphPolicy, ColorMapConcept>(
                default(IndexedAdjacencyListGraphPolicy), colorMapConcept);

            DefaultDfs = new Dfs<IndexedAdjacencyListGraph, int, int,
                List<int>.Enumerator, ColorMap, IndexedAdjacencyListGraphPolicy, ColorMapConcept>(
                default(IndexedAdjacencyListGraphPolicy), colorMapConcept);
        }

        [Params(10, 100, 1000)]
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int VertexCount { get; set; }

        private BaselineDfs<IndexedAdjacencyListGraph, int, int, List<int>.Enumerator, ColorMap,
                IndexedAdjacencyListGraphPolicy, ColorMapConcept>
            BaselineDfs { get; }

        private Dfs<IndexedAdjacencyListGraph, int, int, List<int>.Enumerator, ColorMap,
                IndexedAdjacencyListGraphPolicy, ColorMapConcept>
            DefaultDfs { get; }

        private Dfs<IndexedAdjacencyListGraph, int, int, List<int>.Enumerator, ColorMap,
                IndexedAdjacencyListGraphPolicy, ColorMapConcept>
            CachingDfs { get; set; }

        private IndexedAdjacencyListGraph Graph { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            var indexedMapConcept = new ColorMapConcept(VertexCount);
            indexedMapConcept.Warmup();

            CachingDfs = new Dfs<IndexedAdjacencyListGraph, int, int, List<int>.Enumerator, ColorMap,
                IndexedAdjacencyListGraphPolicy, ColorMapConcept>(
                default(IndexedAdjacencyListGraphPolicy), indexedMapConcept);
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
