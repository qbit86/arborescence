namespace Arborescence.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    /// <summary>
    /// Provides access to the endpoints of each edge, and the out-edges of each vertex
    /// in the simple incidence graph.
    /// </summary>
    public readonly struct SimpleIncidenceGraphPolicy :
        ITailPolicy<SimpleIncidenceGraph, int, uint>,
        IHeadPolicy<SimpleIncidenceGraph, int, uint>,
        IOutEdgesPolicy<SimpleIncidenceGraph, int, ArraySegmentEnumerator<uint>>
    {
        /// <inheritdoc/>
        public bool TryGetTail(SimpleIncidenceGraph graph, uint edge, out int tail) => graph.TryGetTail(edge, out tail);

        /// <inheritdoc/>
        public bool TryGetHead(SimpleIncidenceGraph graph, uint edge, out int head) => graph.TryGetHead(edge, out head);

        /// <inheritdoc/>
        public ArraySegmentEnumerator<uint> EnumerateOutEdges(SimpleIncidenceGraph graph, int vertex) =>
            graph.EnumerateOutEdges(vertex);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
