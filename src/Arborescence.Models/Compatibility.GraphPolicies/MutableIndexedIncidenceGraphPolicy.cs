namespace Arborescence.Models.Compatibility
{
    using System;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    /// <summary>
    /// Provides access to the endpoints of each edge, and the out-edges of each vertex
    /// in the mutable indexed incidence graph.
    /// </summary>
    public readonly struct MutableIndexedIncidenceGraphPolicy :
        ITailPolicy<MutableIndexedIncidenceGraph, int, int>,
        IHeadPolicy<MutableIndexedIncidenceGraph, int, int>,
        IOutEdgesPolicy<MutableIndexedIncidenceGraph, int, ArrayPrefixEnumerator<int>>
    {
        /// <inheritdoc/>
        public bool TryGetTail(MutableIndexedIncidenceGraph graph, int edge, out int tail)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));

            return graph.TryGetTail(edge, out tail);
        }

        /// <inheritdoc/>
        public bool TryGetHead(MutableIndexedIncidenceGraph graph, int edge, out int head)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));

            return graph.TryGetHead(edge, out head);
        }

        /// <inheritdoc/>
        public ArrayPrefixEnumerator<int> EnumerateOutEdges(MutableIndexedIncidenceGraph graph, int vertex)
        {
            return graph is null ? ArrayPrefixEnumerator<int>.Empty : graph.EnumerateOutEdges(vertex);
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
