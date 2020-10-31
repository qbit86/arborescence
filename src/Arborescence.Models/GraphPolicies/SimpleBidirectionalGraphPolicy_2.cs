namespace Arborescence.Models
{
    /// <summary>
    /// Provides access to the endpoints of each edge, and the in- and out-edges of each vertex
    /// in the simple bidirectional graph.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TEdges">The type of the edges enumerator.</typeparam>
    public readonly struct SimpleBidirectionalGraphPolicy<TGraph, TEdges> :
        ITailPolicy<TGraph, int, Endpoints>,
        IHeadPolicy<TGraph, int, Endpoints>,
        IOutEdgesPolicy<TGraph, int, TEdges>,
        IInEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IBidirectionalGraph<int, Endpoints, TEdges>
    {
        /// <inheritdoc/>
        public bool TryGetTail(TGraph graph, Endpoints edge, out int tail) => graph.TryGetTail(edge, out tail);

        /// <inheritdoc/>
        public bool TryGetHead(TGraph graph, Endpoints edge, out int head) => graph.TryGetHead(edge, out head);

        /// <inheritdoc/>
        public TEdges EnumerateOutEdges(TGraph graph, int vertex) => graph.EnumerateOutEdges(vertex);

        /// <inheritdoc/>
        public TEdges EnumerateInEdges(TGraph graph, int vertex) => graph.EnumerateInEdges(vertex);
    }
}
