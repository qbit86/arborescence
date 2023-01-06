namespace Arborescence
{
    /// <inheritdoc cref="IHeadIncidence{TVertex, TEdge}"/>
    /// <inheritdoc cref="IOutEdgesIncidence{TVertex, TEdges}"/>
    public interface IForwardIncidence<TVertex, in TEdge, out TEdges> :
        IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdges> { }
}
