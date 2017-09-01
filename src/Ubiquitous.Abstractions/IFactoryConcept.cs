namespace Ubiquitous
{
    public interface IFactoryConcept<TGraph, TValue>
    {
        TValue Acquire(TGraph graph);
        void Release(TGraph graph, TValue map);
    }
}
