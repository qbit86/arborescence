namespace Ubiquitous
{
    using System;
    using System.Buffers;
    using System.Collections;
    using System.Collections.Generic;
    using BenchmarkDotNet.Attributes;
    using Models;
    using Traversal;
    using EdgeEnumerator = ArraySegmentEnumerator<int>;
    using IndexedAdjacencyListGraphPolicy =
        Models.IndexedIncidenceGraphPolicy<Models.AdjacencyListIncidenceGraph, ArraySegmentEnumerator<int>>;

    [MemoryDiagnoser]
    public abstract class BfsBenchmark
    {
        private readonly DummyHandler<AdjacencyListIncidenceGraph> _handler =
            new DummyHandler<AdjacencyListIncidenceGraph>();

        private byte[] _colorMap = Array.Empty<byte>();
        private BitArray _exploredSet;

        [Params(10, 100, 1000)]
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int VertexCount { get; set; }

        private InstantBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[],
                IndexedAdjacencyListGraphPolicy, IndexedColorMapPolicy>
            InstantBfs { get; set; }

        private EnumerableBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, BitArray,
                IndexedAdjacencyListGraphPolicy, IndexedSetPolicy>
            EnumerableBfs { get; set; }

        private AdjacencyListIncidenceGraph Graph { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            IndexedAdjacencyListGraphPolicy graphPolicy = default;
            IndexedColorMapPolicy colorMapPolicy = default;
            IndexedSetPolicy exploredSetPolicy = default;

            InstantBfs = InstantBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, byte[]>
                .Create(graphPolicy, colorMapPolicy);

            EnumerableBfs = EnumerableBfs<AdjacencyListIncidenceGraph, int, int, EdgeEnumerator, BitArray>
                .Create(graphPolicy, exploredSetPolicy);

            _colorMap = ArrayPool<byte>.Shared.Rent(Graph.VertexCount);
            _exploredSet = new BitArray(Graph.VertexCount);
            _handler.Reset();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            ArrayPool<byte>.Shared.Return(_colorMap, true);
            _colorMap = Array.Empty<byte>();
            _exploredSet = null;
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
            _exploredSet.SetAll(false);
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
            _exploredSet.SetAll(false);
            IEnumerator<int> steps = EnumerableBfs.EnumerateVertices(Graph, 0, _exploredSet);
            int count = 0;
            while (steps.MoveNext())
                ++count;

            steps.Dispose();
            return count;
        }
    }
}
