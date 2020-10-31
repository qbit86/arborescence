namespace Arborescence
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using BenchmarkDotNet.Attributes;
    using Models;
    using Traversal;
    using EdgeEnumerator = System.ArraySegment<int>.Enumerator;

    [MemoryDiagnoser]
    public abstract class CompactSetBenchmark
    {
        private byte[] _compactExploredSet = Array.Empty<byte>();
        private byte[] _fastExploredSet = Array.Empty<byte>();

        protected CompactSetBenchmark()
        {
            CompactDfs = default;
            FastDfs = default;
        }

        [Params(10, 100, 1000, 10000)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int VertexCount { get; set; }

        private EnumerableDfs<IndexedIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedIncidenceGraphPolicy, CompactSetPolicy>
            CompactDfs { get; }

        private EnumerableDfs<IndexedIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedIncidenceGraphPolicy, IndexedSetPolicy>
            FastDfs { get; }

        private IndexedIncidenceGraph Graph { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            _compactExploredSet = ArrayPool<byte>.Shared.Rent(CompactSetPolicy.GetByteCount(Graph.VertexCount));
            _fastExploredSet = ArrayPool<byte>.Shared.Rent(Graph.VertexCount);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            ArrayPool<byte>.Shared.Return(_compactExploredSet, true);
            _compactExploredSet = Array.Empty<byte>();
            ArrayPool<byte>.Shared.Return(_fastExploredSet, true);
            _fastExploredSet = Array.Empty<byte>();
        }

        [Benchmark(Baseline = true)]
        public int Fast()
        {
            Array.Clear(_fastExploredSet, 0, _fastExploredSet.Length);
            using IEnumerator<int> steps = FastDfs.EnumerateEdges(Graph, 0, _fastExploredSet);
            int count = 0;
            while (steps.MoveNext())
                ++count;

            return count;
        }

        [Benchmark]
        public int Compact()
        {
            Array.Clear(_compactExploredSet, 0, _compactExploredSet.Length);
            using IEnumerator<int> steps = CompactDfs.EnumerateEdges(Graph, 0, _compactExploredSet);
            int count = 0;
            while (steps.MoveNext())
                ++count;

            return count;
        }
    }
}
