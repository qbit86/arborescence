namespace Ubiquitous
{
    public interface IBidirectionalGraph<TVertex, in TEdge, out TEdges> : IGraph<TVertex, TEdge>,
        IBidirectional<TVertex, TEdges> { }
}
