namespace Ubiquitous
{
    /// <summary>
    /// Default concept for the type of edge values.
    /// </summary>
    /// <typeparam name="TVertexKey">The type of vertex descriptors.</typeparam>
    /// <typeparam name="TEdgeValue">The type of edge associated data.</typeparam>
    public interface IEdgeConcept<TVertexKey, TEdgeValue>
    {
        TVertexKey GetSource(TEdgeValue edgeValue);
        TVertexKey GetTarget(TEdgeValue edgeValue);
    }
}
