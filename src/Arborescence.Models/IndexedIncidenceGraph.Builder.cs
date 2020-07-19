namespace Arborescence.Models
{
    using System;

    public readonly partial struct IndexedIncidenceGraph
    {
        /// <inheritdoc/>
        public sealed class Builder : IGraphBuilder<IndexedIncidenceGraph, int, int>
        {
            /// <inheritdoc/>
            public bool TryAdd(int tail, int head, out int edge) => throw new NotImplementedException();

            /// <inheritdoc/>
            public IndexedIncidenceGraph ToGraph() => throw new NotImplementedException();
        }
    }
}
