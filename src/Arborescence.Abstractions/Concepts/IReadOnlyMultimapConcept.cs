namespace Arborescence
{
    /// <summary>
    /// Represents data structures that map keys to sequences of values.
    /// </summary>
    /// <typeparam name="TMultimap">The type of the collection.</typeparam>
    /// <typeparam name="TKey">The type of keys.</typeparam>
    /// <typeparam name="TValues">The type of sequences of values.</typeparam>
    public interface IReadOnlyMultimapConcept<in TMultimap, in TKey, out TValues>
    {
        /// <summary>
        /// Gets the sequence of values indexed by a specified key.
        /// </summary>
        /// <param name="multimap">The collection to take the values from.</param>
        /// <param name="key">The key of the desired sequence of values.</param>
        /// <returns>The sequence of values indexed by the specified key.</returns>
        TValues EnumerateValues(TMultimap multimap, TKey key);

        /// <summary>
        /// Gets the number of key/values pairs in the <paramref name="multimap"/>.
        /// </summary>
        /// <param name="multimap">The collection in which to count elements.</param>
        /// <returns>The number of key/values pairs in the <paramref name="multimap"/>.</returns>
        int GetCount(TMultimap multimap);
    }
}
