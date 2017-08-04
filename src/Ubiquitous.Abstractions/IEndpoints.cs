namespace Ubiquitous
{
    public interface IEndpoints<TVertexKey>
    {
        TVertexKey Source { get; }
        TVertexKey Target { get; }
    }
}
