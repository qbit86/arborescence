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
        private byte[] _colorMap = Array.Empty<byte>();

        [Params(10, 100, 1000)]
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int VertexCount { get; set; }

        private InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            InstantDfs { get; set; }

        private IterativeDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            EnumerableDfs { get; set; }

        private AdjacencyListIncidenceGraph Graph { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            var graphPolicy = default(IndexedAdjacencyListGraphPolicy);
            var colorMapPolicy = default(IndexedColorMapPolicy);

            InstantDfs = InstantDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[]>
                .Create(graphPolicy, colorMapPolicy);

            EnumerableDfs = IterativeDfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[]>
                .Create(graphPolicy, colorMapPolicy);

            _colorMap = ArrayPool<byte>.Shared.Rent(Graph.VertexCount);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            ArrayPool<byte>.Shared.Return(_colorMap, true);
            _colorMap = Array.Empty<byte>();
        }

        [Benchmark(Baseline = true)]
        public int InstantDfsTree()
        {
            Array.Clear(_colorMap, 0, _colorMap.Length);
            var handler = new DummyDfsHandler<AdjacencyListIncidenceGraph>();
            InstantDfs.Traverse(Graph, 0, _colorMap, handler);
            return handler.Count;
        }

        [Benchmark]
        public int EnumerableDfsEdgesTree()
        {
            Array.Clear(_colorMap, 0, _colorMap.Length);
            var steps = EnumerableDfs.EnumerateEdges(Graph, 0, _colorMap);
            int count = 0;
            while (steps.MoveNext())
                ++count;

            steps.Dispose();
            return count;
        }

        [Benchmark]
        public int EnumerableDfsVerticesTree()
        {
            Array.Clear(_colorMap, 0, _colorMap.Length);
            var steps = EnumerableDfs.EnumerateVertices(Graph, 0, _colorMap);
            int count = 0;
            while (steps.MoveNext())
                ++count;

            steps.Dispose();
            return count;
        }
    }
}
