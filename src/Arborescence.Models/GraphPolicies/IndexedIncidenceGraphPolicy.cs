namespace Arborescence.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    /// <summary>
    /// Provides access to the endpoints of each edge, and the out-edges of each vertex
    /// in the indexed incidence graph.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TEdges">The type of the edges enumerator.</typeparam>
    public readonly struct IndexedIncidenceGraphPolicy<TGraph, TEdges> :
        ITailPolicy<TGraph, int, int>,
        IHeadPolicy<TGraph, int, int>,
        IOutEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IIncidenceGraph<int, int, TEdges>
    {
        /// <inheritdoc/>
        public bool TryGetTail(TGraph graph, int edge, out int tail) => graph.TryGetTail(edge, out tail);

        /// <inheritdoc/>
        public bool TryGetHead(TGraph graph, int edge, out int head) => graph.TryGetHead(edge, out head);

        /// <inheritdoc/>
        public TEdges EnumerateOutEdges(TGraph graph, int vertex) => graph.EnumerateOutEdges(vertex);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
