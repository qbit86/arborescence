namespace Arborescence.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    internal static class IncidenceAdjacencyAdapter<TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        internal static IncidenceAdjacencyAdapter<TVertex, TEdge, TEdgeEnumerator, TGraph> Create<TGraph>(TGraph graph)
            where TGraph : ITailIncidence<TVertex, TEdge>, IHeadIncidence<TVertex, TEdge>,
            IOutEdgesIncidence<TVertex, TEdgeEnumerator> =>
            new(graph);
    }

    internal sealed class IncidenceAdjacencyAdapter<TVertex, TEdge, TEdgeEnumerator, TGraph> :
        IAdjacency<TVertex, IEnumerator<TVertex>>, ITailIncidence<TVertex, TEdge>, IHeadIncidence<TVertex, TEdge>,
        IOutEdgesIncidence<TVertex, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraph : ITailIncidence<TVertex, TEdge>, IHeadIncidence<TVertex, TEdge>,
        IOutEdgesIncidence<TVertex, TEdgeEnumerator>
    {
        private readonly TGraph _graph;

        public IncidenceAdjacencyAdapter(TGraph graph) =>
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));

        public IEnumerator<TVertex> EnumerateOutNeighbors(TVertex vertex) => EnumerateNeighborsIterator(vertex);

        public bool TryGetHead(TEdge edge, [MaybeNullWhen(false)] out TVertex head) =>
            _graph.TryGetHead(edge, out head);

        public TEdgeEnumerator EnumerateOutEdges(TVertex vertex) => _graph.EnumerateOutEdges(vertex);

        public bool TryGetTail(TEdge edge, [MaybeNullWhen(false)] out TVertex tail) =>
            _graph.TryGetTail(edge, out tail);

        private IEnumerator<TVertex> EnumerateNeighborsIterator(TVertex vertex)
        {
            TEdgeEnumerator edgeEnumerator = _graph.EnumerateOutEdges(vertex);
            try
            {
                while (edgeEnumerator.MoveNext())
                {
                    if (_graph.TryGetHead(edgeEnumerator.Current, out TVertex? neighbor))
                        yield return neighbor;
                }
            }
            finally
            {
                edgeEnumerator.Dispose();
            }
        }
    }
}
