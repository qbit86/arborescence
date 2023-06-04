namespace Arborescence
{
    /// <summary>
    /// Provides a method to add an element with the provided key and value to the <typeparamref name="TDictionary"/>.
    /// </summary>
    /// <typeparam name="TDictionary">The type of the dictionary.</typeparam>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public interface IDictionaryAddition<in TDictionary, in TKey, in TValue>
    {
        /// <summary>
        /// Adds an element with the provided key and value to the <paramref name="dictionary"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary to add an element to.</param>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        void Add(TDictionary dictionary, TKey key, TValue value);
    }
}
