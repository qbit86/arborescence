namespace Arborescence.Models
{
    using System;

    public readonly partial struct SimpleIncidenceGraph
    {
        /// <inheritdoc/>
        public sealed class OrderedBuilder : IGraphBuilder<SimpleIncidenceGraph, int, uint>
        {
            /// <inheritdoc/>
            public bool TryAdd(int tail, int head, out uint edge) => throw new NotImplementedException();

            /// <inheritdoc/>
            public SimpleIncidenceGraph ToGraph() => throw new NotImplementedException();
        }
    }
}
