namespace Ubiquitous
{
    public interface IGetEndpointsPolicy<in TGraph, TVertex, in TEdge>
    {
        bool TryGetEndpoints(TGraph graph, TEdge edge, out SourceTargetPair<TVertex> endpoints);
    }
}
