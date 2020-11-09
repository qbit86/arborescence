namespace Arborescence
{
    public interface IReadOnlyMapPolicy<in TMap, in TKey, TValue>
    {
        /// <summary>
        /// Gets the value associated with the specified key from the map.
        /// </summary>
        /// <param name="map">The map to get from.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">
        /// When this method returns, contains the value associated with the specified key, if the key is found;
        /// otherwise, the unspecified value.
        /// </param>
        /// <returns>A value indicating whether the key was found successfully.</returns>
        bool TryGetValue(TMap map, TKey key, out TValue value);
    }
}
