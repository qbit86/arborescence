namespace Ubiquitous
{
    public interface IBidirectionalGraph<TVertex, in TEdge, TEdges> : IGraph<TVertex, TEdge>,
        IBidirectional<TVertex, TEdges>
    {
    }
}
