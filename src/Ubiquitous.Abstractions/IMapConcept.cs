namespace Ubiquitous
{
    public interface IMapConcept<in TMap, in TKey, TValue>
    {
        bool TryGet(TMap map, TKey key, out TValue value);
        void Put(TMap map, TKey key, TValue value);
    }
}
