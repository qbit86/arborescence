// ReSharper disable SuggestVarOrType_Elsewhere

namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using BenchmarkDotNet.Attributes;
    using Models;
    using Traversal;
    using ColorMapPolicy = Models.IndexedMapPolicy<Traversal.Color>;
    using EdgeEnumerator = ArraySegmentEnumerator<int>;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    [MemoryDiagnoser]
    public abstract class DfsTreeBoostBenchmark
    {
        [Params(10, 100, 1000)]
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int VertexCount { get; set; }

        private InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            InstantDfs { get; set; }

        private Dfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, Color[], IndexedDfsStep,
                IndexedAdjacencyListGraphPolicy, ColorMapPolicy, IndexedDfsStepPolicy>
            DefaultDfs { get; set; }

        private Dfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, Color[], IndexedDfsStep,
                IndexedAdjacencyListGraphPolicy, ColorMapPolicy, IndexedDfsStepPolicy>
            CachingDfs { get; set; }

        private AdjacencyListIncidenceGraph Graph { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            var colorMapPolicy = new ColorMapPolicy(VertexCount);

            InstantDfs = InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[]>
                .Create(default(IndexedAdjacencyListGraphPolicy), default(IndexedColorMapPolicy));

            DefaultDfs = Dfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, Color[], IndexedDfsStep>
                .Create(default(IndexedAdjacencyListGraphPolicy), colorMapPolicy, default(IndexedDfsStepPolicy));

            var indexedMapPolicy = new ColorMapPolicy(VertexCount);
            indexedMapPolicy.Warmup();

            CachingDfs = Dfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, Color[], IndexedDfsStep>
                .Create(default(IndexedAdjacencyListGraphPolicy), indexedMapPolicy, default(IndexedDfsStepPolicy));
        }

        [Benchmark(Baseline = true)]
        public int BaselineDfsTree()
        {
            var handler = new DummyDfsHandler<AdjacencyListIncidenceGraph>();
            byte[] colorMap = ArrayPool<byte>.Shared.Rent(Graph.VertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            InstantDfs.Traverse(Graph, 0, colorMap, handler);
            ArrayPool<byte>.Shared.Return(colorMap);
            return handler.Count;
        }

        [Benchmark]
        public int DefaultDfsTree()
        {
            int count = 0;
            var steps = DefaultDfs.Traverse(Graph, 0);
            foreach (IndexedDfsStep _ in steps)
                ++count;

            return count;
        }

        [Benchmark]
        public int CachingDfsTree()
        {
            int count = 0;
            var steps = CachingDfs.Traverse(Graph, 0);
            foreach (IndexedDfsStep _ in steps)
                ++count;

            return count;
        }
    }
}
