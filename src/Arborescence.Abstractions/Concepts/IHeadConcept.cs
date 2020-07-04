namespace Arborescence
{
    /// <summary>
    /// Represents an incidence function mapping the edge to its head.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface IHeadConcept<TVertex, in TEdge>
    {
        bool TryGetHead(TEdge edge, out TVertex head);
    }
}
