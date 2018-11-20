namespace Ubiquitous
{
    public interface IGetEndpoints<TVertex, in TEdge>
    {
        bool TryGetEndpoints(TEdge edge, out SourceTargetPair<TVertex> endpoints);
    }
}
