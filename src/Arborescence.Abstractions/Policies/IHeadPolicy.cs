namespace Arborescence
{
    /// <summary>
    /// Defines an incidence function mapping the edge to its head.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    public interface IHeadPolicy<in TGraph, TVertex, in TEdge>
    {
        bool TryGetHead(TGraph graph, TEdge edge, out TVertex head);
    }
}
