namespace Ubiquitous
{
    using System.Collections.Generic;

    public interface IMapFactoryConcept<TKey, TValue>
    {
        IDictionary<TKey, TValue> Acquire();
        void Release(IDictionary<TKey, TValue> map);
    }
}
