namespace Arborescence
{
    public interface ITailConcept<TVertex, in TEdge>
    {
        bool TryGetTail(TEdge edge, out TVertex tail);
    }
}
