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
    public abstract class BfsBenchmark
    {
        private readonly DummyHandler<IndexedIncidenceGraph> _handler =
            new DummyHandler<IndexedIncidenceGraph>();

        private byte[] _colorMap = Array.Empty<byte>();
        private byte[] _exploredSet = Array.Empty<byte>();

        [Params(10, 100, 1000)]
        public int VertexCount { get; set; }

        private InstantBfs<IndexedIncidenceGraph, int, int, EdgeEnumerator, byte[], IndexedColorMapPolicy>
            InstantBfs { get; set; }

        private EnumerableBfs<IndexedIncidenceGraph, int, int, EdgeEnumerator, byte[], IndexedSetPolicy>
            EnumerableBfs { get; set; }

        private IndexedIncidenceGraph Graph { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            InstantBfs = default;
            EnumerableBfs = default;

            _colorMap = ArrayPool<byte>.Shared.Rent(Graph.VertexCount);
            _exploredSet = ArrayPool<byte>.Shared.Rent(Graph.VertexCount);
            _handler.Reset();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            ArrayPool<byte>.Shared.Return(_colorMap, true);
            _colorMap = Array.Empty<byte>();
            ArrayPool<byte>.Shared.Return(_exploredSet, true);
            _exploredSet = Array.Empty<byte>();
        }

        [Benchmark(Baseline = true)]
        public int InstantBfsSteps()
        {
            Array.Clear(_colorMap, 0, _colorMap.Length);
            InstantBfs.Traverse(Graph, 0, _colorMap, _handler);
            return _handler.Count;
        }

        [Benchmark]
        public int EnumerableBfsEdges()
        {
            Array.Clear(_exploredSet, 0, _exploredSet.Length);
            IEnumerator<int> steps = EnumerableBfs.EnumerateEdges(Graph, 0, _exploredSet);
            int count = 0;
            while (steps.MoveNext())
                ++count;

            steps.Dispose();
            return count;
        }

        [Benchmark]
        public int EnumerableBfsVertices()
        {
            Array.Clear(_exploredSet, 0, _exploredSet.Length);
            IEnumerator<int> steps = EnumerableBfs.EnumerateVertices(Graph, 0, _exploredSet);
            int count = 0;
            while (steps.MoveNext())
                ++count;

            steps.Dispose();
            return count;
        }
    }
}
