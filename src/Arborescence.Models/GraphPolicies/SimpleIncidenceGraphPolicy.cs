#if NETSTANDARD2_1 || NETCOREAPP2_0 || NETCOREAPP2_1
namespace Arborescence.Models
{
    using System;

    /// <summary>
    /// Provides access to the endpoints of each edge, and the out-edges of each vertex
    /// in the simple incidence graph.
    /// </summary>
    public readonly struct SimpleIncidenceGraphPolicy :
        ITailPolicy<SimpleIncidenceGraph, int, Endpoints>,
        IHeadPolicy<SimpleIncidenceGraph, int, Endpoints>,
        IOutEdgesPolicy<SimpleIncidenceGraph, int, ArraySegment<Endpoints>.Enumerator>
    {
        /// <inheritdoc/>
        public bool TryGetTail(SimpleIncidenceGraph graph, Endpoints edge, out int tail) =>
            graph.TryGetTail(edge, out tail);

        /// <inheritdoc/>
        public bool TryGetHead(SimpleIncidenceGraph graph, Endpoints edge, out int head) =>
            graph.TryGetHead(edge, out head);

        /// <inheritdoc/>
        public ArraySegment<Endpoints>.Enumerator EnumerateOutEdges(SimpleIncidenceGraph graph, int vertex) =>
            graph.EnumerateOutEdges(vertex);
    }
}
#endif
