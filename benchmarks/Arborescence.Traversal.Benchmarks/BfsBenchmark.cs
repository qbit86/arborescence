﻿namespace Arborescence
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
        private readonly DummyHandler<IndexedIncidenceGraph> _handler = new();

        private byte[] _colorByVertex = Array.Empty<byte>();
        private byte[] _exploredSet = Array.Empty<byte>();

        [Params(10, 100, 1000)]
        public int VertexCount { get; set; }

        private EagerBfs<IndexedIncidenceGraph, int, int, EdgeEnumerator> EagerBfs { get; set; }

        private EnumerableBfs<IndexedIncidenceGraph, int, int, EdgeEnumerator> EnumerableBfs { get; set; }

        private IndexedIncidenceGraph Graph { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Graph = GraphHelper.Default.GetGraph(VertexCount);

            EagerBfs = default;
            EnumerableBfs = default;

            _colorByVertex = ArrayPool<byte>.Shared.Rent(Graph.VertexCount);
            _exploredSet = ArrayPool<byte>.Shared.Rent(Graph.VertexCount);
            _handler.Reset();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            ArrayPool<byte>.Shared.Return(_colorByVertex, true);
            _colorByVertex = Array.Empty<byte>();
            ArrayPool<byte>.Shared.Return(_exploredSet, true);
            _exploredSet = Array.Empty<byte>();
        }

        [Benchmark(Baseline = true)]
        public int EagerBfsSteps()
        {
            Array.Clear(_colorByVertex, 0, _colorByVertex.Length);
            EagerBfs.Traverse(Graph, 0, new IndexedColorDictionary(_colorByVertex), _handler);
            return _handler.Count;
        }

        [Benchmark]
        public int EnumerableBfsEdges()
        {
            Array.Clear(_exploredSet, 0, _exploredSet.Length);
            using IEnumerator<int> steps = EnumerableBfs.EnumerateEdges(Graph, 0, new IndexedSet(_exploredSet));
            int count = 0;
            while (steps.MoveNext())
                ++count;

            return count;
        }

        [Benchmark]
        public int EnumerableBfsVertices()
        {
            Array.Clear(_exploredSet, 0, _exploredSet.Length);
            using IEnumerator<int> steps = EnumerableBfs.EnumerateVertices(Graph, 0, new IndexedSet(_exploredSet));
            int count = 0;
            while (steps.MoveNext())
                ++count;

            return count;
        }
    }
}
