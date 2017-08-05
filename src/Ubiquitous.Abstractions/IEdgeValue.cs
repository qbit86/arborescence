namespace Ubiquitous
{
    /// <summary>
    /// Default constraint for the type of edge values.
    /// </summary>
    /// <typeparam name="TVertexKey">The type of vertex descriptors.</typeparam>
    public interface IEdgeValue<TVertexKey>
    {
        TVertexKey Source { get; }
        TVertexKey Target { get; }
    }
}
