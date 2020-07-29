namespace Arborescence.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    /// <summary>
    /// Provides access to the endpoints of each edge, and the out-edges of each vertex
    /// in the simple incidence graph.
    /// </summary>
    public readonly struct SimpleIncidenceGraphPolicy :
        ITailPolicy<SimpleIncidenceGraph, int, Endpoints>,
        IHeadPolicy<SimpleIncidenceGraph, int, Endpoints>,
        IOutEdgesPolicy<SimpleIncidenceGraph, int, ArraySegmentEnumerator<Endpoints>>
    {
        /// <inheritdoc/>
        public bool TryGetTail(SimpleIncidenceGraph graph, Endpoints edge, out int tail) =>
            graph.TryGetTail(edge, out tail);

        /// <inheritdoc/>
        public bool TryGetHead(SimpleIncidenceGraph graph, Endpoints edge, out int head) =>
            graph.TryGetHead(edge, out head);

        /// <inheritdoc/>
        public ArraySegmentEnumerator<Endpoints> EnumerateOutEdges(SimpleIncidenceGraph graph, int vertex) =>
            graph.EnumerateOutEdges(vertex);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
