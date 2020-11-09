namespace Arborescence
{
    /// <summary>
    /// Defines methods to support getting and putting items for the map.
    /// </summary>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IMapPolicy<in TMap, in TKey, TValue> : IReadOnlyMapPolicy<TMap, TKey, TValue>
    {
        /// <summary>
        /// Adds the key and value if the key doesn't exist, or updates the existing key's value if it does exist.
        /// </summary>
        /// <param name="map">The map to put to.</param>
        /// <param name="key">The key to add or update.</param>
        /// <param name="value">The value to associate with the key.</param>
        void AddOrUpdate(TMap map, TKey key, TValue value);
    }
}
