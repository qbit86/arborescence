namespace Arborescence.Traversal.Specialized
{
    using System;
    using System.Collections.Generic;

#pragma warning disable CA1815
    /// <summary>
    /// Represents the BFS algorithm — breadth-first traversal of the graph — implemented as enumerator.
    /// </summary>
    /// <remarks>
    /// This implementation assumes vertices to be indices in range [0, vertexCount).
    /// </remarks>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    [Obsolete]
    public readonly partial struct EnumerableBfs<TGraph, TEdge, TEdgeEnumerator>
        where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge> { }
#pragma warning restore CA1815
}
