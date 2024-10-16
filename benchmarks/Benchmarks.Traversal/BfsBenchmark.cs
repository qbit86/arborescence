﻿namespace Arborescence;

using System;
using System.Buffers;
using BenchmarkDotNet.Attributes;
using Models.Specialized;
using Traversal.Incidence;
using EdgeEnumerator = System.ArraySegment<int>.Enumerator;

[MemoryDiagnoser]
public abstract class BfsBenchmark
{
    private readonly DummyHandler<Int32IncidenceGraph> _handler = new();

    private byte[] _colorByVertex = Array.Empty<byte>();
    private byte[] _exploredSet = Array.Empty<byte>();

    [Params(10, 100, 1000)]
    public int VertexCount { get; set; }

    private Int32IncidenceGraph Graph { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Graph = GraphHelper.Default.GetIncidenceGraph(VertexCount);

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
        EagerBfs<int, int, EdgeEnumerator>.Traverse(Graph, 0, new Int32ColorDictionary(_colorByVertex), _handler);
        return _handler.Count;
    }

    [Benchmark]
    public int EnumerableBfsEdges()
    {
        Array.Clear(_exploredSet, 0, _exploredSet.Length);
        using var steps = EnumerableBfs<int, int, EdgeEnumerator>.EnumerateEdges(
            Graph, 0, new Int32Set(_exploredSet)).GetEnumerator();
        int count = 0;
        while (steps.MoveNext())
            ++count;

        return count;
    }

    [Benchmark]
    public int EnumerableBfsVertices()
    {
        Array.Clear(_exploredSet, 0, _exploredSet.Length);
        using var steps = EnumerableBfs<int, int, EdgeEnumerator>.EnumerateVertices(
            Graph, 0, new Int32Set(_exploredSet)).GetEnumerator();
        int count = 0;
        while (steps.MoveNext())
            ++count;

        return count;
    }
}
