namespace Ubiquitous
{
    public interface IMapFactoryConcept<TGraph, TValue>
    {
        TValue Acquire(TGraph graph);
        void Release(TGraph graph, TValue map);
    }
}
