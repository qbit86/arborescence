namespace Ubiquitous
{
    public interface IGraph<TVertex, in TEdge>
    {
        bool TryGetSource(TEdge edge, out TVertex source);
        bool TryGetTarget(TEdge edge, out TVertex target);
    }
}
