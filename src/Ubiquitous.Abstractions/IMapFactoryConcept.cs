namespace Ubiquitous
{
    public interface IMapFactoryConcept<TGraph, TKey, TValue, TMap>
    {
        TMap Acquire(TGraph graph);
        void Release(TGraph graph, TMap map);
    }
}
