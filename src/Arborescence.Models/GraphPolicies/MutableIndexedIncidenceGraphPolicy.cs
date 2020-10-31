#if NETSTANDARD2_1 || NETCOREAPP2_0 || NETCOREAPP2_1
namespace Arborescence.Models
{
    using System;

    /// <summary>
    /// Provides access to the endpoints of each edge, and the out-edges of each vertex
    /// in the mutable indexed incidence graph.
    /// </summary>
    public readonly struct MutableIndexedIncidenceGraphPolicy :
        ITailPolicy<MutableIndexedIncidenceGraph, int, int>,
        IHeadPolicy<MutableIndexedIncidenceGraph, int, int>,
        IOutEdgesPolicy<MutableIndexedIncidenceGraph, int, ArraySegment<int>.Enumerator>
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
        public ArraySegment<int>.Enumerator EnumerateOutEdges(MutableIndexedIncidenceGraph graph, int vertex)
        {
            return graph is null ? ArraySegment<int>.Empty.GetEnumerator() : graph.EnumerateOutEdges(vertex);
        }
    }
}
#endif
