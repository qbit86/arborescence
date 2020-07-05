namespace Arborescence
{
    /// <summary>
    /// Represents a graph as a pair of incidence functions.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface IGraph<TVertex, in TEdge> :
        IHeadConcept<TVertex, TEdge>, ITailConcept<TVertex, TEdge> { }
}
