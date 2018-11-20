namespace Ubiquitous
{
    public interface IGetTarget<TVertex, in TEdge>
    {
        bool TryGetTarget(TEdge edge, out TVertex target);
    }
}
