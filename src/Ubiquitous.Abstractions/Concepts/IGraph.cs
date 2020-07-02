namespace Ubiquitous
{
    public interface IGraph<TVertex, in TEdge> : IHeadConcept<TVertex, TEdge>, ITailConcept<TVertex, TEdge> { }
}
