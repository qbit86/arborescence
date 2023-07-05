namespace Arborescence
{
    using System.Collections.Generic;
    using Primitives;

    /// <summary>
    /// Provides a set of initialization methods for instances
    /// of the <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/> type.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public static class Int32IndirectDictionaryFactory<TKey, TValue>
    {
        /// <summary>
        /// Creates an <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// with the specified mapping from a key to an index and list of values.
        /// </summary>
        /// <param name="indexByKey">The mapping from a key to an index.</param>
        /// <param name="values">The underlying list of the values.</param>
        /// <typeparam name="TKeyToIndexMap">The type of the mapping from a key to an <see cref="int"/>.</typeparam>
        /// <typeparam name="TValueList">The type of the backing list.</typeparam>
        /// <returns>
        /// An <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// that contains the specified values.
        /// </returns>
        public static Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, Int32Dictionary<TValue, TValueList>>
            CreateFromList<TKeyToIndexMap, TValueList>(TKeyToIndexMap indexByKey, TValueList values)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TValueList : IList<TValue>
        {
            if (indexByKey is null)
                ArgumentNullExceptionHelpers.Throw(nameof(indexByKey));
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            Int32Dictionary<TValue, TValueList> valueByIndex = new(values);
            return new(indexByKey, valueByIndex);
        }

        /// <summary>
        /// Creates an <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// with the specified mapping from a key to an index, list of values, and absence marker.
        /// </summary>
        /// <param name="indexByKey">The mapping from a key to an index.</param>
        /// <param name="values">The underlying list of the values.</param>
        /// <param name="absenceMarker">The object to use as the absence marker.</param>
        /// <typeparam name="TKeyToIndexMap">The type of the mapping from a key to an <see cref="int"/>.</typeparam>
        /// <typeparam name="TValueList">The type of the backing list.</typeparam>
        /// <returns>
        /// An <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// that contains the specified values.
        /// </returns>
        public static Int32IndirectDictionary<
                TKey, TValue, TKeyToIndexMap, Int32Dictionary<TValue, TValueList, EqualityComparer<TValue>>>
            CreateFromListWithAbsence<TKeyToIndexMap, TValueList>(
                TKeyToIndexMap indexByKey, TValueList values, TValue? absenceMarker = default)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TValueList : IList<TValue>
        {
            if (indexByKey is null)
                ArgumentNullExceptionHelpers.Throw(nameof(indexByKey));
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            Int32Dictionary<TValue, TValueList, EqualityComparer<TValue>> valueByIndex =
                new(values, EqualityComparer<TValue>.Default, absenceMarker);
            return new(indexByKey, valueByIndex);
        }

        /// <summary>
        /// Creates an <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// with the specified list of values
        /// and the absence marker to compare values against with the specified comparer.
        /// </summary>
        /// <param name="indexByKey">The mapping from a key to an index.</param>
        /// <param name="values">The underlying list of the values.</param>
        /// <param name="absenceComparer">The comparer to compare the values with the absence marker.</param>
        /// <param name="absenceMarker">The object to use as the absence marker.</param>
        /// <typeparam name="TKeyToIndexMap">The type of the mapping from a key to an <see cref="int"/>.</typeparam>
        /// <typeparam name="TValueList">The type of the backing list.</typeparam>
        /// <typeparam name="TAbsenceComparer">The type that provides a method to distinguish missing elements.</typeparam>
        /// <returns>
        /// A <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// that contains the specified values.
        /// </returns>
        public static Int32IndirectDictionary<
                TKey, TValue, TKeyToIndexMap, Int32Dictionary<TValue, TValueList, TAbsenceComparer>>
            CreateFromListWithAbsence<TKeyToIndexMap, TValueList, TAbsenceComparer>(
                TKeyToIndexMap indexByKey, TValueList values, TAbsenceComparer absenceComparer,
                TValue? absenceMarker = default)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TValueList : IList<TValue>
            where TAbsenceComparer : IEqualityComparer<TValue>
        {
            if (indexByKey is null)
                ArgumentNullExceptionHelpers.Throw(nameof(indexByKey));
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            if (absenceComparer is null)
                ArgumentNullExceptionHelpers.Throw(nameof(absenceComparer));
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> valueByIndex =
                new(values, absenceComparer, absenceMarker);
            return new(indexByKey, valueByIndex);
        }

        /// <summary>
        /// Creates an <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// with the specified mapping from a key to an index and mapping from an index to a value.
        /// </summary>
        /// <param name="indexByKey">The mapping from a key to an index.</param>
        /// <param name="valueByIndex">The mapping from an index to a value.</param>
        /// <typeparam name="TKeyToIndexMap">The type of the mapping from a key to an <see cref="int"/>.</typeparam>
        /// <typeparam name="TIndexToValueMap">The type of the mapping from an <see cref="int"/> to a value.</typeparam>
        /// <returns>
        /// An <see cref="Int32IndirectDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>.
        /// </returns>
        public static Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>
            Create<TKeyToIndexMap, TIndexToValueMap>(TKeyToIndexMap indexByKey, TIndexToValueMap valueByIndex)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TIndexToValueMap : IDictionary<int, TValue>
        {
            if (indexByKey is null)
                ArgumentNullExceptionHelpers.Throw(nameof(indexByKey));
            if (valueByIndex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(valueByIndex));
            return new(indexByKey, valueByIndex);
        }
    }
}
