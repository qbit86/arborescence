namespace Arborescence
{
    /// <summary>
    /// Represents an incidence function mapping the edge to its tail.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface ITailConcept<TVertex, in TEdge>
    {
        bool TryGetTail(TEdge edge, out TVertex tail);
    }
}
