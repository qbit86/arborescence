namespace Ubiquitous
{
    using System.Collections.Generic;

    public interface IMapFactoryConcept<TGraph, TKey, TValue>
    {
        IDictionary<TKey, TValue> Acquire(TGraph graph);
        void Release(TGraph graph, IDictionary<TKey, TValue> map);
    }
}
