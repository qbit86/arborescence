namespace Arborescence.Models
{
    using System.Collections.Generic;

    public static class ReadOnlyMultimapConceptFactory<TMultimap, TKey, TValueCollection, TValueEnumerator>
        where TMultimap : IReadOnlyDictionary<TKey, TValueCollection>
    {
        public static ReadOnlyMultimapConcept<
            TMultimap, TKey, TValueCollection, TValueEnumerator, TEnumeratorProvider> Create<TEnumeratorProvider>(
            TEnumeratorProvider enumeratorProvider)
            where TEnumeratorProvider : IEnumeratorProvider<TValueCollection, TValueEnumerator>
        {
            if (enumeratorProvider is null)
                ThrowHelper.ThrowArgumentNullException(nameof(enumeratorProvider));

            return new(enumeratorProvider);
        }
    }

    public readonly struct ReadOnlyMultimapConcept<
        TMultimap, TKey, TValueCollection, TValueEnumerator, TEnumeratorProvider> :
        IReadOnlyMultimapConcept<TMultimap, TKey, TValueEnumerator>
        where TMultimap : IReadOnlyDictionary<TKey, TValueCollection>
        where TEnumeratorProvider : IEnumeratorProvider<TValueCollection, TValueEnumerator>
    {
        private readonly TEnumeratorProvider _enumeratorProvider;

        internal ReadOnlyMultimapConcept(TEnumeratorProvider enumeratorProvider) =>
            _enumeratorProvider = enumeratorProvider;

        public TValueEnumerator EnumerateValues(TMultimap multimap, TKey key) =>
            ReadOnlyMultimapHelpers<TValueCollection, TValueEnumerator>.GetEnumerator(
                multimap, key, _enumeratorProvider);

        public int GetCount(TMultimap multimap) => multimap.Count;
    }
}
