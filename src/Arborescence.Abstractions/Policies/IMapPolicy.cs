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
        /// <summary>
        /// Gets the value associated with the specified key from the <paramref name="map"/>.
        /// </summary>
        /// <param name="map">The map to get from.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">
        /// When this method returns, contains the value associated with the specified key, if the key is found;
        /// otherwise, the unspecified value.
        /// </param>
        /// <returns>A value indicating whether the key was found successfully.</returns>
        bool TryGetValue(TMap map, TKey key, out TValue value);

        /// <summary>
        /// Adds the key and value if the key doesn't exist, or updates the existing key's value if it does exist.
        /// </summary>
        /// <param name="map">The map to put to.</param>
        /// <param name="key">The key to add or update.</param>
        /// <param name="value">The value to associate with <paramref name="key"/>.</param>
        void AddOrUpdate(TMap map, TKey key, TValue value);
    }
}
