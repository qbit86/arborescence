namespace Ubiquitous
{
    public interface IGraph<TVertex, in TEdge>
    {
        bool TryGetTail(TEdge edge, out TVertex source);
        bool TryGetHead(TEdge edge, out TVertex target);
    }
}
