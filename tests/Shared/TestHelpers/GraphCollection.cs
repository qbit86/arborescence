namespace Arborescence;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Models;
using Models.Specialized;
using Workbench;

internal abstract class GraphCollection<TGraph, TEdge, TEdges, TGraphBuilder> : IEnumerable<object[]>
    where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdges>
    where TGraphBuilder : IGraphBuilder<TGraph, int, TEdge>
{
    private const int LowerBound = 1;
    private const int UpperBound = 13;

    private static CultureInfo P => CultureInfo.InvariantCulture;

    public IEnumerator<object[]> GetEnumerator()
    {
        for (int i = LowerBound; i < UpperBound; ++i)
        {
            string testCase = i.ToString("D2", CultureInfo.InvariantCulture);
            var builder = CreateGraphBuilder(0);

            using (var textReader = IndexedGraphs.GetTextReader(testCase))
            {
                if (textReader == TextReader.Null)
                    continue;

                var edges = Base32EdgeListParser.ParseEdges(textReader);
                foreach (var edge in edges)
                    builder.TryAdd(edge.Tail, edge.Head, out _);
            }

            var graph = builder.ToGraph();
            string description = $"{{{nameof(testCase)}: {testCase}}}";
            var graphParameter = GraphParameter.Create(graph, description);
            yield return new object[] { graphParameter };
        }

        {
            const int vertexCount = 1;
            const double densityPower = 1.0;
            var builder = CreateGraphBuilder(1);
            GraphHelpers.PopulateIncidenceGraphBuilder<TGraph, TEdge, TEdges, TGraphBuilder>(
                builder, vertexCount, densityPower);
            var graph = builder.ToGraph();
            string description =
                $"{{{nameof(vertexCount)}: {vertexCount.ToString(P)}, {nameof(densityPower)}: {densityPower.ToString(P)}}}";
            var graphParameter = GraphParameter.Create(graph, description);
            yield return new object[] { graphParameter };
        }

        for (int i = 1; i < 7; ++i)
        {
            double power = 0.5 * i;
            int vertexCount = (int)Math.Ceiling(Math.Pow(10.0, power));
            foreach (double densityPower in GraphHelpers.DensityPowers)
            {
                var builder = CreateGraphBuilder(1);
                GraphHelpers.PopulateIncidenceGraphBuilder<TGraph, TEdge, TEdges, TGraphBuilder>(
                    builder, vertexCount, densityPower);
                var graph = builder.ToGraph();
                string description =
                    $"{{{nameof(vertexCount)}: {vertexCount.ToString(P)}, {nameof(densityPower)}: {densityPower.ToString(P)}}}";
                var graphParameter = GraphParameter.Create(graph, description);
                yield return new object[] { graphParameter };
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected abstract TGraphBuilder CreateGraphBuilder(int initialVertexCount);
}

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
internal sealed class Int32AdjacencyGraphCollection : GraphCollection<
    Int32AdjacencyGraph,
    Endpoints<int>,
    IncidenceEnumerator<int, ArraySegment<int>.Enumerator>,
    Int32AdjacencyGraphBuilder>
{
    protected override Int32AdjacencyGraphBuilder CreateGraphBuilder(int initialVertexCount) => new();
}

internal sealed class ListAdjacencyGraphCollection : GraphCollection<
    ListAdjacencyGraph<int, Int32Dictionary<List<int>, List<List<int>>>>,
    Endpoints<int>,
    IncidenceEnumerator<int, List<int>.Enumerator>,
    ListAdjacencyGraphBuilder>
{
    protected override ListAdjacencyGraphBuilder CreateGraphBuilder(int initialVertexCount) => new(initialVertexCount);
}

internal sealed class Int32IncidenceGraphCollection : GraphCollection<
    Int32IncidenceGraph,
    int,
    ArraySegment<int>.Enumerator,
    Int32IncidenceGraphBuilder>
{
    protected override Int32IncidenceGraphBuilder CreateGraphBuilder(int initialVertexCount) => new();
}

internal sealed class ListIncidenceGraphCollection : GraphCollection<
    ListIncidenceGraph<int, int, Int32Dictionary<int, List<int>>, Dictionary<int, List<int>>>,
    int,
    List<int>.Enumerator,
    ListIncidenceGraphBuilder>
{
    protected override ListIncidenceGraphBuilder CreateGraphBuilder(int initialVertexCount) => new(initialVertexCount);
}

internal sealed class Int32AdjacencyGraphBuilder : IGraphBuilder<Int32AdjacencyGraph, int, Endpoints<int>>
{
    private readonly List<Endpoints<int>> _edges = new();

    public bool TryAdd(int tail, int head, out Endpoints<int> edge)
    {
        edge = new(tail, head);
        _edges.Add(edge);
        return true;
    }

    public Int32AdjacencyGraph ToGraph()
    {
        var edges = _edges.ToArray();
        _edges.Clear();
        return Int32AdjacencyGraph.FromEdges(edges);
    }
}

internal sealed class ListAdjacencyGraphBuilder : IGraphBuilder<
    ListAdjacencyGraph<int, Int32Dictionary<List<int>, List<List<int>>>>,
    int,
    Endpoints<int>>
{
    private readonly Int32Dictionary<List<int>, List<List<int>>> _neighborsByVertex;

    internal ListAdjacencyGraphBuilder(int initialVertexCount) =>
        _neighborsByVertex = Int32Dictionary<List<int>>.Create(new List<List<int>>(initialVertexCount));

    public bool TryAdd(int tail, int head, out Endpoints<int> edge)
    {
        int max = Math.Max(tail, head);
        while (max >= _neighborsByVertex.Count)
            _neighborsByVertex.Add(_neighborsByVertex.Count, default!);
        if (_neighborsByVertex[tail] is { } neighbors)
            neighbors.Add(head);
        else
            _neighborsByVertex[tail] = new() { head };

        edge = new(tail, head);
        return true;
    }

    public ListAdjacencyGraph<int, Int32Dictionary<List<int>, List<List<int>>>> ToGraph() =>
        ListAdjacencyGraph<int>.Create(_neighborsByVertex);
}

internal sealed class Int32IncidenceGraphBuilder : IGraphBuilder<Int32IncidenceGraph, int, int>
{
    private readonly List<Endpoints<int>> _endpointsByEdge = new();

    public bool TryAdd(int tail, int head, out int edge)
    {
        edge = _endpointsByEdge.Count;
        _endpointsByEdge.Add(new(tail, head));
        return true;
    }

    public Int32IncidenceGraph ToGraph()
    {
        var endpointsByEdge = _endpointsByEdge.ToArray();
        _endpointsByEdge.Clear();
        return Int32IncidenceGraph.FromEdges(endpointsByEdge);
    }
}

internal sealed class ListIncidenceGraphBuilder : IGraphBuilder<
    ListIncidenceGraph<int, int, Int32Dictionary<int, List<int>>, Dictionary<int, List<int>>>,
    int,
    int>
{
    private readonly Int32Dictionary<int, List<int>> _headByEdge;
    private readonly Dictionary<int, List<int>> _outEdgesByVertex;
    private readonly Int32Dictionary<int, List<int>> _tailByEdge;

    internal ListIncidenceGraphBuilder(int initialVertexCount)
    {
        _tailByEdge = Int32Dictionary<int>.Create(new List<int>());
        _headByEdge = Int32Dictionary<int>.Create(new List<int>());
        _outEdgesByVertex = new(initialVertexCount);
    }

    public bool TryAdd(int tail, int head, out int edge)
    {
        Debug.Assert(_tailByEdge.Count == _headByEdge.Count);
        int max = Math.Max(tail, head);
        while (max >= _outEdgesByVertex.Count)
            _outEdgesByVertex.Add(_outEdgesByVertex.Count, default!);
        edge = _headByEdge.Count;
        _tailByEdge.Add(edge, tail);
        _headByEdge.Add(edge, head);
        if (_outEdgesByVertex[tail] is { } outEdges)
            outEdges.Add(edge);
        else
            _outEdgesByVertex[tail] = new() { edge };
        return true;
    }

    public ListIncidenceGraph<int, int, Int32Dictionary<int, List<int>>, Dictionary<int, List<int>>> ToGraph() =>
        ListIncidenceGraph<int, int>.CreateUnchecked(_tailByEdge, _headByEdge, _outEdgesByVertex);
}
#endif
