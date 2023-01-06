namespace Arborescence
{
    /// <summary>
    /// Represents a traversable concept — an access to the head of each edge and to the out-edges of each vertex.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdges">The type of the edges enumerator.</typeparam>
    public interface IForwardIncidence<TVertex, in TEdge, out TEdges> :
        IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdges> { }
}
