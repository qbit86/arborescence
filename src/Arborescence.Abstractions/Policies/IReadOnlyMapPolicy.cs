namespace Arborescence
{
#if NETSTANDARD2_1 || NETCOREAPP3_1
    using System.Diagnostics.CodeAnalysis;

#endif

    /// <summary>
    /// Defines a method to get items from a map.
    /// </summary>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
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
#if NETSTANDARD2_1 || NETCOREAPP3_1
        bool TryGetValue(TMap map, TKey key, [MaybeNullWhen(false)] out TValue value);
#else
        bool TryGetValue(TMap map, TKey key, out TValue value);
#endif
    }
}
