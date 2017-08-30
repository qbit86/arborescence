namespace Ubiquitous
{
    /// <summary>
    /// Represents a generic graph concept with access to vertex and edge associated data.
    /// </summary>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    /// <typeparam name="TVertexKey">The type of vertex descriptors.</typeparam>
    /// <typeparam name="TEdgeKey">The type of edge descriptors.</typeparam>
    /// <typeparam name="TVertexData">The type of vertex associated data.</typeparam>
    /// <typeparam name="TEdgeData">The type of edge associated data.</typeparam>
    public interface IGraphConcept<TGraph, TVertexKey, TEdgeKey, TVertexData, TEdgeData>
    {
        bool TryGetVertexData(TGraph graph, TVertexKey vertex, out TVertexData vertexData);
        bool TryGetEdgeData(TGraph graph, TEdgeKey edge, out TEdgeData edgeData);
    }
}
