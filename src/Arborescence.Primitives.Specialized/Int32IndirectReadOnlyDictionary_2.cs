namespace Arborescence
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of initialization methods for instances
    /// of the <see cref="Int32IndirectReadOnlyDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/> type.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public static class Int32IndirectReadOnlyDictionary<TKey, TValue>
    {
        /// <summary>
        /// Creates an <see cref="Int32IndirectReadOnlyDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// with the specified mapping from a key to an index and list of values.
        /// </summary>
        /// <param name="indexByKey">The mapping from a key to an index.</param>
        /// <param name="values">The underlying list of the values.</param>
        /// <typeparam name="TKeyToIndexMap">The type of the mapping from a key to an <see cref="int"/>.</typeparam>
        /// <typeparam name="TValueList">The type of the backing list.</typeparam>
        /// <returns>
        /// An <see cref="Int32IndirectReadOnlyDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// that contains the specified values.
        /// </returns>
        public static Int32IndirectReadOnlyDictionary<
                TKey, TValue, TKeyToIndexMap, Int32ReadOnlyDictionary<TValue, TValueList>>
            CreateFromList<TKeyToIndexMap, TValueList>(TKeyToIndexMap indexByKey, TValueList values)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TValueList : IReadOnlyList<TValue>
        {
            if (indexByKey is null)
                ArgumentNullExceptionHelpers.Throw(nameof(indexByKey));
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            Int32ReadOnlyDictionary<TValue, TValueList> valueByIndex = new(values);
            return new(indexByKey, valueByIndex);
        }

        /// <summary>
        /// Creates an <see cref="Int32IndirectReadOnlyDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// with the specified mapping from a key to an index, list of values, and the default absence marker.
        /// </summary>
        /// <param name="indexByKey">The mapping from a key to an index.</param>
        /// <param name="values">The underlying list of the values.</param>
        /// <typeparam name="TKeyToIndexMap">The type of the mapping from a key to an <see cref="int"/>.</typeparam>
        /// <typeparam name="TValueList">The type of the backing list.</typeparam>
        /// <returns>
        /// An <see cref="Int32IndirectReadOnlyDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// that contains the specified values.
        /// </returns>
        public static Int32IndirectReadOnlyDictionary<
                TKey, TValue, TKeyToIndexMap, Int32ReadOnlyDictionary<TValue, TValueList, DefaultAbsence<TValue>>>
            CreateFromListWithAbsence<TKeyToIndexMap, TValueList>(
                TKeyToIndexMap indexByKey, TValueList values)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TValueList : IReadOnlyList<TValue>
        {
            if (indexByKey is null)
                ArgumentNullExceptionHelpers.Throw(nameof(indexByKey));
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            Int32ReadOnlyDictionary<TValue, TValueList, DefaultAbsence<TValue>> valueByIndex =
                new(values, default);
            return new(indexByKey, valueByIndex);
        }

        /// <summary>
        /// Creates an <see cref="Int32IndirectReadOnlyDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// with the specified mapping from a key to an index, list of values, and absence marker.
        /// </summary>
        /// <param name="indexByKey">The mapping from a key to an index.</param>
        /// <param name="values">The underlying list of the values.</param>
        /// <param name="absenceEquatable">The object that provides a method for distinguishing missing elements.</param>
        /// <typeparam name="TKeyToIndexMap">The type of the mapping from a key to an <see cref="int"/>.</typeparam>
        /// <typeparam name="TValueList">The type of the backing list.</typeparam>
        /// <typeparam name="TEquatable">The type that provides a method for distinguishing missing elements.</typeparam>
        /// <returns>
        /// An <see cref="Int32IndirectReadOnlyDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// that contains the specified values.
        /// </returns>
        public static Int32IndirectReadOnlyDictionary<
                TKey, TValue, TKeyToIndexMap, Int32ReadOnlyDictionary<TValue, TValueList, TEquatable>>
            CreateFromListWithAbsence<TKeyToIndexMap, TValueList, TEquatable>(
                TKeyToIndexMap indexByKey, TValueList values, TEquatable absenceEquatable)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TValueList : IReadOnlyList<TValue>
            where TEquatable : IEquatable<TValue>
        {
            if (indexByKey is null)
                ArgumentNullExceptionHelpers.Throw(nameof(indexByKey));
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            if (absenceEquatable is null)
                ArgumentNullExceptionHelpers.Throw(nameof(absenceEquatable));
            Int32ReadOnlyDictionary<TValue, TValueList, TEquatable> valueByIndex = new(values, absenceEquatable);
            return new(indexByKey, valueByIndex);
        }

        /// <summary>
        /// Creates an <see cref="Int32IndirectReadOnlyDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>
        /// with the specified mapping from a key to an index and mapping from an index to a value.
        /// </summary>
        /// <param name="indexByKey">The mapping from a key to an index.</param>
        /// <param name="valueByIndex">The mapping from an index to a value.</param>
        /// <typeparam name="TKeyToIndexMap">The type of the mapping from a key to an <see cref="int"/>.</typeparam>
        /// <typeparam name="TIndexToValueMap">The type of the mapping from an <see cref="int"/> to a value.</typeparam>
        /// <returns>
        /// An <see cref="Int32IndirectReadOnlyDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/>.
        /// </returns>
        public static Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>
            Create<TKeyToIndexMap, TIndexToValueMap>(TKeyToIndexMap indexByKey, TIndexToValueMap valueByIndex)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TIndexToValueMap : IReadOnlyDictionary<int, TValue>
        {
            if (indexByKey is null)
                ArgumentNullExceptionHelpers.Throw(nameof(indexByKey));
            if (valueByIndex is null)
                ArgumentNullExceptionHelpers.Throw(nameof(valueByIndex));
            return new(indexByKey, valueByIndex);
        }
    }
}
