namespace Arborescence
{
    /// <summary>
    /// Defines an incidence function mapping the edge to its tail.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface ITailPolicy<in TGraph, TVertex, in TEdge>
    {
        bool TryGetTail(TGraph graph, TEdge edge, out TVertex tail);
    }
}
