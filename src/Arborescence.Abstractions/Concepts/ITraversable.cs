namespace Arborescence
{
    using System;

    /// <summary>
    /// Represents the concept of traversing along the edges of a graph —
    /// an access to the head of each edge and to the outgoing edges of each vertex.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdges">The type of the edges enumerator.</typeparam>
    [Obsolete(
        "ITraversable<TVertex, TEdge, TEdges> has been deprecated. Use IForwardIncidence<TVertex, TEdge, TEdges> instead.")]
    public interface ITraversable<TVertex, in TEdge, out TEdges> :
        IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdges> { }
}
