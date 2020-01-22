namespace Ubiquitous.Traversal
{
    public interface IDfsHandler<in TGraph, in TVertex>
    {
        void StartVertex(TGraph g, TVertex v);
        void DiscoverVertex(TGraph g, TVertex v);
    }
}
