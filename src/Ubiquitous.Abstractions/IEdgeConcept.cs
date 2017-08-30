namespace Ubiquitous
{
    /// <summary>
    /// Default concept for the type of edge values.
    /// </summary>
    /// <typeparam name="TVertexKey">The type of vertex descriptors.</typeparam>
    /// <typeparam name="TEdgeData">The type of edge associated data.</typeparam>
    public interface IEdgeConcept<TVertexKey, TEdgeData>
    {
        TVertexKey GetSource(TEdgeData edgeData);
        TVertexKey GetTarget(TEdgeData edgeData);
    }
}
