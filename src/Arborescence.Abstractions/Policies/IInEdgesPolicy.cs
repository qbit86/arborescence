namespace Arborescence
{
    /// <summary>
    /// Defines an enumeration method for the in-edges of the vertex.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdges">The type of the edges enumerator.</typeparam>
    public interface IInEdgesPolicy<in TGraph, in TVertex, out TEdges>
    {
        TEdges EnumerateInEdges(TGraph graph, TVertex vertex);
    }
}
