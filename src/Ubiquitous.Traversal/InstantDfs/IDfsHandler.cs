namespace Ubiquitous.Traversal
{
    public interface IDfsHandler<in TGraph, in TVertex>
    {
        void StartVertex(TGraph g, TVertex v);
    }
}
