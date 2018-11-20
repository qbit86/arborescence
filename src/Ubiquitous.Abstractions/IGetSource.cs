namespace Ubiquitous
{
    public interface IGetSource<TVertex, in TEdge>
    {
        bool TryGetSource(TEdge edge, out TVertex source);
    }
}
