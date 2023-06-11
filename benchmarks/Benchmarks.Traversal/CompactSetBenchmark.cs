namespace Arborescence;

using System;
using System.Buffers;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Models.Specialized;
using EdgeEnumerator = System.ArraySegment<int>.Enumerator;
using EnumerableDfs = Traversal.Incidence.EnumerableDfs<int, int, System.ArraySegment<int>.Enumerator>;

[MemoryDiagnoser]
public abstract class CompactSetBenchmark
{
    private byte[] _compactExploredSet = Array.Empty<byte>();
    private byte[] _fastExploredSet = Array.Empty<byte>();

    [Params(10, 100, 1000, 10000)]
    public int VertexCount { get; set; }

    private Int32IncidenceGraph Graph { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Graph = GraphHelper.Default.GetIncidenceGraph(VertexCount);

        _compactExploredSet = ArrayPool<byte>.Shared.Rent(CompactSet.GetByteCount(Graph.VertexCount));
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
        IEnumerable<int> steps = EnumerableDfs.EnumerateEdges(Graph, 0, new Int32Set(_fastExploredSet));
        int count = 0;
        foreach (int _ in steps)
            ++count;

        return count;
    }

    [Benchmark]
    public int Compact()
    {
        Array.Clear(_compactExploredSet, 0, _compactExploredSet.Length);
        IEnumerable<int> steps = EnumerableDfs.EnumerateEdges(Graph, 0, new CompactSet(_compactExploredSet));
        int count = 0;
        foreach (int _ in steps)
            ++count;

        return count;
    }
}
