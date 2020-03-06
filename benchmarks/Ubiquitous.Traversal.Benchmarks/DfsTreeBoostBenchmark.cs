namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using BenchmarkDotNet.Attributes;
    using Models;
    using Traversal;
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

        private Dfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[], IndexedDfsStep,
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy, IndexedDfsStepPolicy>
            DefaultDfs { get; set; }

        private AdjacencyListIncidenceGraph Graph { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            var colorMapPolicy = default(IndexedColorMapPolicy);

            InstantDfs = InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[]>
                .Create(default(IndexedAdjacencyListGraphPolicy), colorMapPolicy);

            DefaultDfs = Dfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[], IndexedDfsStep>
                .Create(default(IndexedAdjacencyListGraphPolicy), colorMapPolicy, default(IndexedDfsStepPolicy));
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
            byte[] colorMap = ArrayPool<byte>.Shared.Rent(Graph.VertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            var steps = DefaultDfs.Traverse(Graph, 0, colorMap);
            foreach (IndexedDfsStep _ in steps)
                ++count;

            ArrayPool<byte>.Shared.Return(colorMap);
            return count;
        }
    }
}
