namespace Arborescence
{
    /// <summary>
    /// Defines methods to support getting and putting items for the map.
    /// </summary>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IMapPolicy<in TMap, in TKey, TValue>
    {
        bool TryGetValue(TMap map, TKey key, out TValue value);
        void AddOrUpdate(TMap map, TKey key, TValue value);
    }
}
