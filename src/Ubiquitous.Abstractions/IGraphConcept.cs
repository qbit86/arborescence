namespace Ubiquitous
{
    /// <summary>
    /// Represents a generic graph concept with access to vertex and edge associated data.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertexKey">The type of vertex descriptors.</typeparam>
    /// <typeparam name="TEdgeKey">The type of edge descriptors.</typeparam>
    /// <typeparam name="TVertexValue">The type of vertex associated data.</typeparam>
    /// <typeparam name="TEdgeValue">The type of edge associated data.</typeparam>
    public interface IGraphConcept<TGraph, TVertexKey, TEdgeKey, TVertexValue, TEdgeValue>
    {
        bool TryGetVertexValue(TGraph graph, TVertexKey vertex, out TVertexValue vertexValue);
        bool TryGetEdgeValue(TGraph graph, TEdgeKey edge, out TEdgeValue edgeValue);
    }
}
