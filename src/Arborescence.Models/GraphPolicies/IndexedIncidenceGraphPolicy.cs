#if NETSTANDARD2_1 || NETCOREAPP2_0 || NETCOREAPP2_1

namespace Arborescence.Models
{
    using System;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    /// <summary>
    /// Provides access to the endpoints of each edge, and the out-edges of each vertex
    /// in the indexed incidence graph.
    /// </summary>
    public readonly struct IndexedIncidenceGraphPolicy :
        ITailPolicy<IndexedIncidenceGraph, int, int>,
        IHeadPolicy<IndexedIncidenceGraph, int, int>,
        IOutEdgesPolicy<IndexedIncidenceGraph, int, ArraySegment<int>.Enumerator>
    {
        /// <inheritdoc/>
        public bool TryGetTail(IndexedIncidenceGraph graph, int edge, out int tail) => graph.TryGetTail(edge, out tail);

        /// <inheritdoc/>
        public bool TryGetHead(IndexedIncidenceGraph graph, int edge, out int head) => graph.TryGetHead(edge, out head);

        /// <inheritdoc/>
        public ArraySegment<int>.Enumerator EnumerateOutEdges(IndexedIncidenceGraph graph, int vertex) =>
            graph.EnumerateOutEdges(vertex);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}

#endif
