namespace Arborescence.Models
{
    using System;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
    public sealed class MutableUndirectedIndexedIncidenceGraph :
        IIncidenceGraph<int, int, ArrayPrefixEnumerator<int>>,
        IGraphBuilder<IndexedIncidenceGraph, int, int>,
        IDisposable
    {
        /// <inheritdoc/>
        public void Dispose() => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool TryAdd(int tail, int head, out int edge) => throw new NotImplementedException();

        /// <inheritdoc/>
        public IndexedIncidenceGraph ToGraph() => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool TryGetHead(int edge, out int head) => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool TryGetTail(int edge, out int tail) => throw new NotImplementedException();

        /// <inheritdoc/>
        public ArrayPrefixEnumerator<int> EnumerateOutEdges(int vertex) => throw new NotImplementedException();
    }
}
