namespace Arborescence;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Models;
using Workbench;
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
using IndexedEnumerator = System.ArraySegment<int>.Enumerator;
using SimpleEnumerator = System.ArraySegment<Int32Endpoints>.Enumerator;
#else
using Models.Compatibility;
using IndexedEnumerator = System.Collections.Generic.IEnumerator<int>;
using SimpleEnumerator = System.Collections.Generic.IEnumerator<Int32Endpoints>;
#endif

internal abstract class GraphCollection<TGraph, TEdge, TEdges, TGraphBuilder> : IEnumerable<object[]>
    where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdges>
    where TGraphBuilder : IGraphBuilder<TGraph, int, TEdge>
{
    private const int LowerBound = 1;
    private const int UpperBound = 10;

    private static CultureInfo F => CultureInfo.InvariantCulture;

    public IEnumerator<object[]> GetEnumerator()
    {
        for (int i = LowerBound; i < UpperBound; ++i)
        {
            string testCase = i.ToString("D2", CultureInfo.InvariantCulture);
            TGraphBuilder builder = CreateGraphBuilder(0);

            using (TextReader textReader = IndexedGraphs.GetTextReader(testCase))
            {
                if (textReader == TextReader.Null)
                    continue;

                IEnumerable<Int32Endpoints> edges = IndexedEdgeListParser.ParseEdges(textReader);
                foreach (Int32Endpoints edge in edges)
                    builder.TryAdd(edge.Tail, edge.Head, out _);
            }

            TGraph graph = builder.ToGraph();
            string description = $"{{{nameof(testCase)}: {testCase}}}";
            var graphParameter = GraphParameter.Create(graph, description);
            yield return new object[] { graphParameter };
        }

        {
            const int vertexCount = 1;
            const double densityPower = 1.0;
            TGraphBuilder builder = CreateGraphBuilder(1);
            GraphHelpers.PopulateIncidenceGraphBuilder<TGraph, TEdge, TEdges, TGraphBuilder>(
                builder, vertexCount, densityPower);
            TGraph graph = builder.ToGraph();
            string description =
                $"{{{nameof(vertexCount)}: {vertexCount.ToString(F)}, {nameof(densityPower)}: {densityPower.ToString(F)}}}";
            var graphParameter = GraphParameter.Create(graph, description);
            yield return new object[] { graphParameter };
        }

        for (int i = 1; i < 7; ++i)
        {
            double power = 0.5 * i;
            int vertexCount = (int)Math.Ceiling(Math.Pow(10.0, power));
            foreach (double densityPower in GraphHelpers.DensityPowers)
            {
                TGraphBuilder builder = CreateGraphBuilder(1);
                GraphHelpers.PopulateIncidenceGraphBuilder<TGraph, TEdge, TEdges, TGraphBuilder>(
                    builder, vertexCount, densityPower);
                TGraph graph = builder.ToGraph();
                string description =
                    $"{{{nameof(vertexCount)}: {vertexCount.ToString(F)}, {nameof(densityPower)}: {densityPower.ToString(F)}}}";
                var graphParameter = GraphParameter.Create(graph, description);
                yield return new object[] { graphParameter };
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected abstract TGraphBuilder CreateGraphBuilder(int initialVertexCount);
}

internal sealed class IndexedGraphCollection :
    GraphCollection<IndexedIncidenceGraph, int, IndexedEnumerator, IndexedIncidenceGraph.Builder>
{
    protected override IndexedIncidenceGraph.Builder CreateGraphBuilder(int initialVertexCount) =>
        new(initialVertexCount);
}

internal sealed class FromMutableIndexedGraphCollection :
    GraphCollection<IndexedIncidenceGraph, int, IndexedEnumerator, MutableIndexedIncidenceGraph>
{
    protected override MutableIndexedIncidenceGraph CreateGraphBuilder(int initialVertexCount) =>
        new(initialVertexCount);
}

internal sealed class MutableIndexedGraphCollection : GraphCollection<
    MutableIndexedIncidenceGraph, int, IndexedEnumerator, MutableIndexedIncidenceGraphBuilder>
{
    protected override MutableIndexedIncidenceGraphBuilder CreateGraphBuilder(int initialVertexCount) =>
        new(initialVertexCount);
}

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
internal sealed class Int32AdjacencyGraphCollection : GraphCollection<
    Models.Specialized.Int32AdjacencyGraph,
    Endpoints<int>,
    IncidenceEnumerator<int, ArraySegment<int>.Enumerator>,
    Int32AdjacencyGraphBuilder>
{
    protected override Int32AdjacencyGraphBuilder CreateGraphBuilder(int initialVertexCount) => new();
}

internal sealed class ListAdjacencyGraphCollection : GraphCollection<
    ListAdjacencyGraph<int, Dictionary<int, List<int>>>,
    Endpoints<int>,
    IncidenceEnumerator<int, List<int>.Enumerator>,
    ListAdjacencyGraphBuilder>
{
    protected override ListAdjacencyGraphBuilder CreateGraphBuilder(int initialVertexCount) => new(initialVertexCount);
}
#endif

internal sealed class MutableIndexedIncidenceGraphBuilder :
    IGraphBuilder<MutableIndexedIncidenceGraph, int, int>,
    IDisposable
{
    private MutableIndexedIncidenceGraph? _graph;

    public MutableIndexedIncidenceGraphBuilder(int initialVertexCount) => _graph = new(initialVertexCount);

    public void Dispose()
    {
        if (_graph is null)
            return;

        _graph.Dispose();
        _graph = null;
    }

    public bool TryAdd(int tail, int head, out int edge)
    {
        if (_graph is null)
            throw new ObjectDisposedException(nameof(MutableIndexedIncidenceGraphBuilder));

        return _graph.TryAdd(tail, head, out edge);
    }

    public MutableIndexedIncidenceGraph ToGraph()
    {
        if (_graph is null)
            throw new ObjectDisposedException(nameof(MutableIndexedIncidenceGraphBuilder));

        MutableIndexedIncidenceGraph result = _graph;
        _graph = null;
        return result;
    }
}

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
internal sealed class Int32AdjacencyGraphBuilder : IGraphBuilder<
    Models.Specialized.Int32AdjacencyGraph,
    int,
    Endpoints<int>>
{
    private readonly List<Endpoints<int>> _edges = new();

    public bool TryAdd(int tail, int head, out Endpoints<int> edge)
    {
        edge = new(tail, head);
        _edges.Add(edge);
        return true;
    }

    public Models.Specialized.Int32AdjacencyGraph ToGraph()
    {
        Endpoints<int>[] edges = _edges.ToArray();
        return Models.Specialized.Int32AdjacencyGraphFactory.FromEdges(edges);
    }
}

internal sealed class ListAdjacencyGraphBuilder : IGraphBuilder<
    ListAdjacencyGraph<int, Dictionary<int, List<int>>>,
    int,
    Endpoints<int>>
{
    private readonly Dictionary<int, List<int>> _neighborsByVertex;

    public ListAdjacencyGraphBuilder(int initialVertexCount) => _neighborsByVertex = new(initialVertexCount);

    public bool TryAdd(int tail, int head, out Endpoints<int> edge)
    {
        if (_neighborsByVertex.TryGetValue(tail, out List<int>? neighbors))
            neighbors.Add(head);
        else
            _neighborsByVertex.Add(tail, new() { head });

        edge = new(tail, head);
        return true;
    }

    public ListAdjacencyGraph<int, Dictionary<int, List<int>>> ToGraph() =>
        ListAdjacencyGraphFactory<int>.Create(_neighborsByVertex);
}
#endif
