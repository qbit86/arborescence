namespace Arborescence;

using System;
using System.Buffers;
using BenchmarkDotNet.Attributes;
using Models.Specialized;
using Traversal.Incidence;
using EdgeEnumerator = System.ArraySegment<int>.Enumerator;
using EnumerableDfs = Traversal.Incidence.EnumerableDfs<int, int, System.ArraySegment<int>.Enumerator>;

[MemoryDiagnoser]
public abstract class DfsBenchmark
{
    private readonly DummyHandler<Int32IncidenceGraph> _handler = new();

    private byte[] _colorByVertex = Array.Empty<byte>();

    [Params(10, 100, 1000, 10000)]
    public int VertexCount { get; set; }

    private Int32IncidenceGraph Graph { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Graph = GraphHelper.Default.GetIncidenceGraph(VertexCount);

        _colorByVertex = ArrayPool<byte>.Shared.Rent(Graph.VertexCount);
        _handler.Reset();
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        ArrayPool<byte>.Shared.Return(_colorByVertex, true);
        _colorByVertex = Array.Empty<byte>();
    }

    [Benchmark(Baseline = true)]
    public int EagerDfsSteps()
    {
        Array.Clear(_colorByVertex, 0, _colorByVertex.Length);
        EagerDfs<int, int, EdgeEnumerator>.Traverse(Graph, 0, new Int32ColorDictionary(_colorByVertex), _handler);
        return _handler.Count;
    }

    [Benchmark]
    public int RecursiveDfsSteps()
    {
        Array.Clear(_colorByVertex, 0, _colorByVertex.Length);
        RecursiveDfs<int, int, EdgeEnumerator>.Traverse(Graph, 0, new Int32ColorDictionary(_colorByVertex), _handler);
        return _handler.Count;
    }

    [Benchmark]
    public int EnumerableDfsEdges()
    {
        Array.Clear(_colorByVertex, 0, _colorByVertex.Length);
        var steps = EnumerableDfs.EnumerateEdges(Graph, 0, new Int32Set(_colorByVertex));
        int count = 0;
        foreach (int _ in steps)
            ++count;

        return count;
    }

    [Benchmark]
    public int EnumerableDfsVertices()
    {
        Array.Clear(_colorByVertex, 0, _colorByVertex.Length);
        var steps = EnumerableDfs.EnumerateVertices(Graph, 0, new Int32Set(_colorByVertex));
        int count = 0;
        foreach (int _ in steps)
            ++count;

        return count;
    }
}
