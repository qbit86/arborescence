namespace Arborescence.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides support for creating
    /// <see cref="ReadOnlyMultimapConcept{TMultimap, TKey, TValueCollection, TValueEnumerator, TEnumeratorProvider}"/>
    /// objects.
    /// </summary>
    /// <typeparam name="TMultimap">The type of the dictionary.</typeparam>
    /// <typeparam name="TKey">The type of keys.</typeparam>
    /// <typeparam name="TValueCollection">The type of the value collection.</typeparam>
    /// <typeparam name="TValueEnumerator">The type of the value enumerator.</typeparam>
    public static class ReadOnlyMultimapConcept<TMultimap, TKey, TValueCollection, TValueEnumerator>
        where TMultimap : IReadOnlyDictionary<TKey, TValueCollection>
    {
        /// <summary>
        /// Creates a
        /// <see cref="ReadOnlyMultimapConcept{TMultimap, TKey, TValueCollection, TValueEnumerator, TEnumeratorProvider}"/>.
        /// </summary>
        /// <param name="enumeratorProvider">The enumerator provider.</param>
        /// <typeparam name="TEnumeratorProvider">The type of the enumerator provider.</typeparam>
        /// <returns>The object that provides the mapping of keys to sequences of values.</returns>
        public static ReadOnlyMultimapConcept<
            TMultimap, TKey, TValueCollection, TValueEnumerator, TEnumeratorProvider> Create<TEnumeratorProvider>(
            TEnumeratorProvider enumeratorProvider)
            where TEnumeratorProvider : IEnumeratorProvider<TValueCollection, TValueEnumerator>
        {
            if (enumeratorProvider is null)
                ArgumentNullExceptionHelpers.Throw(nameof(enumeratorProvider));

            return new(enumeratorProvider);
        }
    }
}
