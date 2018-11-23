// ReSharper disable SuggestVarOrType_Elsewhere

namespace Ubiquitous
{
    using System.Collections.Generic;
    using BenchmarkDotNet.Attributes;
    using Traversal.Advanced;
    using ColorMap = ArrayPrefix<Traversal.Color>;
    using ColorMapPolicy = IndexedMapPolicy<Traversal.Color>;
    using IndexedAdjacencyListGraphPolicy =
        IndexedIncidenceGraphPolicy<JaggedAdjacencyListIncidenceGraph, ArrayPrefixEnumerator<int>>;

    [MemoryDiagnoser]
    public abstract class DfsTreeBoostBenchmark
    {
        protected DfsTreeBoostBenchmark()
        {
            var colorMapPolicy = new ColorMapPolicy(VertexCount);

            BaselineDfs = new BaselineDfs<JaggedAdjacencyListIncidenceGraph, int, int,
                ArrayPrefixEnumerator<int>, ColorMap, IndexedAdjacencyListGraphPolicy, ColorMapPolicy>(
                default(IndexedAdjacencyListGraphPolicy), colorMapPolicy);

            DefaultDfs = new Dfs<JaggedAdjacencyListIncidenceGraph, int, int,
                ArrayPrefixEnumerator<int>, ColorMap, IndexedAdjacencyListGraphPolicy, ColorMapPolicy>(
                default(IndexedAdjacencyListGraphPolicy), colorMapPolicy);
        }

        [Params(10, 100, 1000)]
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int VertexCount { get; set; }

        private BaselineDfs<JaggedAdjacencyListIncidenceGraph, int, int, ArrayPrefixEnumerator<int>, ColorMap,
                IndexedAdjacencyListGraphPolicy, ColorMapPolicy>
            BaselineDfs { get; }

        private Dfs<JaggedAdjacencyListIncidenceGraph, int, int, ArrayPrefixEnumerator<int>, ColorMap,
                IndexedAdjacencyListGraphPolicy, ColorMapPolicy>
            DefaultDfs { get; }

        private Dfs<JaggedAdjacencyListIncidenceGraph, int, int, ArrayPrefixEnumerator<int>, ColorMap,
                IndexedAdjacencyListGraphPolicy, ColorMapPolicy>
            CachingDfs { get; set; }

        private JaggedAdjacencyListIncidenceGraph Graph { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            var indexedMapPolicy = new ColorMapPolicy(VertexCount);
            indexedMapPolicy.Warmup();

            CachingDfs = new Dfs<JaggedAdjacencyListIncidenceGraph, int, int, ArrayPrefixEnumerator<int>, ColorMap,
                IndexedAdjacencyListGraphPolicy, ColorMapPolicy>(
                default(IndexedAdjacencyListGraphPolicy), indexedMapPolicy);
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
