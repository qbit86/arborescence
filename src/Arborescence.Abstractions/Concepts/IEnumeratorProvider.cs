namespace Arborescence
{
    /// <summary>
    /// Exposes the enumerator, which supports a simple iteration over the <typeparamref name="TCollection"/>.
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection.</typeparam>
    /// <typeparam name="TEnumerator">The type of the enumerator.</typeparam>
    public interface IEnumeratorProvider<in TCollection, out TEnumerator>
    {
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <param name="collection">The collection to iterate through.</param>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        TEnumerator GetEnumerator(TCollection collection);

        /// <summary>
        /// Returns an empty enumeration.
        /// </summary>
        /// <returns>An enumerator that indicates the end of the collection.</returns>
        TEnumerator GetEmptyEnumerator();
    }
}
