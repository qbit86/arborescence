namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the generic search algorithm — traversal of the graph
    /// where the order of exploring vertices is determined by the frontier implementation.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    [Obsolete(
        "GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator> has been deprecated. Use EnumerableGenericSearch<TVertex, TEdge, TEdgeEnumerator> instead.")]
    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator>
        where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge> { }
}
