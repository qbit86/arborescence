namespace Arborescence
{
    /// <summary>
    /// Provides an enumerator for the in-edges of the vertex.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdges">The type of the edges enumerator.</typeparam>
    public interface IInEdgesConcept<in TVertex, out TEdges>
    {
        TEdges EnumerateInEdges(TVertex vertex);
    }
}
