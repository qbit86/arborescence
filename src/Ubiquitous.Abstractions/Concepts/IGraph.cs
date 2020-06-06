namespace Ubiquitous
{
    public interface IGraph<TVertex, in TEdge>
    {
        bool TryGetTail(TEdge edge, out TVertex tail);
        bool TryGetHead(TEdge edge, out TVertex head);
    }
}
