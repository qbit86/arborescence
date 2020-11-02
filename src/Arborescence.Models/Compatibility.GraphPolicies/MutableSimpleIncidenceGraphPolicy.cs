namespace Arborescence.Models.Compatibility
{
    using System;

    /// <summary>
    /// Provides access to the endpoints of each edge, and the out-edges of each vertex
    /// in the mutable simple incidence graph.
    /// </summary>
#if NETSTANDARD2_1 || NETCOREAPP2_0 || NETCOREAPP2_1
    [Obsolete("Please use Arborescence.Models.MutableSimpleIncidenceGraphPolicy instead.")]
#endif
    public readonly struct MutableSimpleIncidenceGraphPolicy :
        ITailPolicy<MutableSimpleIncidenceGraph, int, Endpoints>,
        IHeadPolicy<MutableSimpleIncidenceGraph, int, Endpoints>,
        IOutEdgesPolicy<MutableSimpleIncidenceGraph, int, ArrayPrefixEnumerator<Endpoints>>
    {
        /// <inheritdoc/>
        public bool TryGetTail(MutableSimpleIncidenceGraph graph, Endpoints edge, out int tail)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));

            return graph.TryGetTail(edge, out tail);
        }

        /// <inheritdoc/>
        public bool TryGetHead(MutableSimpleIncidenceGraph graph, Endpoints edge, out int head)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));

            return graph.TryGetHead(edge, out head);
        }

        /// <inheritdoc/>
        public ArrayPrefixEnumerator<Endpoints> EnumerateOutEdges(MutableSimpleIncidenceGraph graph, int vertex)
        {
            return graph is null ? ArrayPrefixEnumerator<Endpoints>.Empty : graph.EnumerateOutEdges(vertex);
        }
    }
}
