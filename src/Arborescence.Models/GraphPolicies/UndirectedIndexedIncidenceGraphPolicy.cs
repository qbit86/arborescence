#if NETSTANDARD2_1 || NETCOREAPP2_0 || NETCOREAPP2_1
namespace Arborescence.Models
{
    using System;

    /// <summary>
    /// Provides access to the endpoints of each edge, and the out-edges of each vertex
    /// in the undirected indexed incidence graph.
    /// </summary>
    public readonly struct UndirectedIndexedIncidenceGraphPolicy :
        ITailPolicy<UndirectedIndexedIncidenceGraph, int, int>,
        IHeadPolicy<UndirectedIndexedIncidenceGraph, int, int>,
        IOutEdgesPolicy<UndirectedIndexedIncidenceGraph, int, ArraySegment<int>.Enumerator>
    {
        /// <inheritdoc/>
        public bool TryGetTail(UndirectedIndexedIncidenceGraph graph, int edge, out int tail) =>
            graph.TryGetTail(edge, out tail);

        /// <inheritdoc/>
        public bool TryGetHead(UndirectedIndexedIncidenceGraph graph, int edge, out int head) =>
            graph.TryGetHead(edge, out head);

        /// <inheritdoc/>
        public ArraySegment<int>.Enumerator EnumerateOutEdges(UndirectedIndexedIncidenceGraph graph, int vertex) =>
            graph.EnumerateOutEdges(vertex);
    }
}
#endif
