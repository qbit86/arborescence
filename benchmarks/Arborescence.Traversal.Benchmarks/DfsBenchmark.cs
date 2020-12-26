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
    public abstract class DfsBenchmark
    {
        private readonly DummyHandler<IndexedIncidenceGraph> _handler = new();

        private byte[] _colorMap = Array.Empty<byte>();

        protected DfsBenchmark()
        {
            EagerDfs = default;
            RecursiveDfs = default;
            EnumerableDfs = default;
        }

        [Params(10, 100, 1000, 10000)]
        public int VertexCount { get; set; }

        private EagerDfs<IndexedIncidenceGraph, int, int, EdgeEnumerator, byte[], IndexedColorMapPolicy>
            EagerDfs { get; }

        private RecursiveDfs<IndexedIncidenceGraph, int, int, EdgeEnumerator, byte[], IndexedColorMapPolicy>
            RecursiveDfs { get; }

        private EnumerableDfs<IndexedIncidenceGraph, int, int, EdgeEnumerator, byte[], IndexedSetPolicy>
            EnumerableDfs { get; }

        private IndexedIncidenceGraph Graph { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            _colorMap = ArrayPool<byte>.Shared.Rent(Graph.VertexCount);
            _handler.Reset();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            ArrayPool<byte>.Shared.Return(_colorMap, true);
            _colorMap = Array.Empty<byte>();
        }

        [Benchmark(Baseline = true)]
        public int EagerDfsSteps()
        {
            Array.Clear(_colorMap, 0, _colorMap.Length);
            EagerDfs.Traverse(Graph, 0, _colorMap, _handler);
            return _handler.Count;
        }

        [Benchmark]
        public int RecursiveDfsSteps()
        {
            Array.Clear(_colorMap, 0, _colorMap.Length);
            RecursiveDfs.Traverse(Graph, 0, _colorMap, _handler);
            return _handler.Count;
        }

        [Benchmark]
        public int EnumerableDfsEdges()
        {
            Array.Clear(_colorMap, 0, _colorMap.Length);
            using IEnumerator<int> steps = EnumerableDfs.EnumerateEdges(Graph, 0, _colorMap);
            int count = 0;
            while (steps.MoveNext())
                ++count;

            return count;
        }

        [Benchmark]
        public int EnumerableDfsVertices()
        {
            Array.Clear(_colorMap, 0, _colorMap.Length);
            using IEnumerator<int> steps = EnumerableDfs.EnumerateVertices(Graph, 0, _colorMap);
            int count = 0;
            while (steps.MoveNext())
                ++count;

            return count;
        }
    }
}
