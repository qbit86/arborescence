namespace Ubiquitous
{
    public interface IHeadConcept<TVertex, in TEdge>
    {
        bool TryGetHead(TEdge edge, out TVertex head);
    }
}
