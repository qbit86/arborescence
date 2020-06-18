namespace Ubiquitous
{
    public interface IMapPolicy<in TMap, in TKey, TValue>
    {
        bool TryGetValue(TMap map, TKey key, out TValue value);
        void AddOrUpdate(TMap map, TKey key, TValue value);
    }
}
