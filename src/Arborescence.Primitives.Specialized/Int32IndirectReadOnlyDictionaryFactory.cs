namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using Primitives;

    /// <summary>
    /// Provides a set of initialization methods for instances
    /// of the <see cref="Int32IndirectReadOnlyDictionary{TKey, TValue, TKeyToIndexMap, TIndexToValueMap}"/> type.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public static class Int32IndirectReadOnlyDictionaryFactory<TKey, TValue>
    {
        public static Int32IndirectReadOnlyDictionary<
                TKey, TValue, TKeyToIndexMap, Int32ReadOnlyDictionary<TValue, TValueList>>
            CreateFromList<TKeyToIndexMap, TValueList>(TKeyToIndexMap indexByKey, TValueList values)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TValueList : IReadOnlyList<TValue>
        {
            if (indexByKey is null)
                ThrowHelper.ThrowArgumentNullException(nameof(indexByKey));
            if (values is null)
                ThrowHelper.ThrowArgumentNullException(nameof(values));
            Int32ReadOnlyDictionary<TValue, TValueList> valueByIndex = new(values);
            return new(indexByKey, valueByIndex);
        }

        public static Int32IndirectReadOnlyDictionary<
                TKey, TValue, TKeyToIndexMap, Int32ReadOnlyDictionary<TValue, TValueList, DefaultAbsence<TValue>>>
            CreateFromListWithAbsence<TKeyToIndexMap, TValueList>(
                TKeyToIndexMap indexByKey, TValueList values)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TValueList : IReadOnlyList<TValue>
        {
            if (indexByKey is null)
                ThrowHelper.ThrowArgumentNullException(nameof(indexByKey));
            if (values is null)
                ThrowHelper.ThrowArgumentNullException(nameof(values));
            Int32ReadOnlyDictionary<TValue, TValueList, DefaultAbsence<TValue>> valueByIndex =
                new(values, default);
            return new(indexByKey, valueByIndex);
        }

        public static Int32IndirectReadOnlyDictionary<
                TKey, TValue, TKeyToIndexMap, Int32ReadOnlyDictionary<TValue, TValueList, TAbsence>>
            CreateFromListWithAbsence<TKeyToIndexMap, TValueList, TAbsence>(
                TKeyToIndexMap indexByKey, TValueList values, TAbsence absence)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TValueList : IReadOnlyList<TValue>
            where TAbsence : IEquatable<TValue>
        {
            if (indexByKey is null)
                ThrowHelper.ThrowArgumentNullException(nameof(indexByKey));
            if (values is null)
                ThrowHelper.ThrowArgumentNullException(nameof(values));
            if (absence is null)
                ThrowHelper.ThrowArgumentNullException(nameof(absence));
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsence> valueByIndex = new(values, absence);
            return new(indexByKey, valueByIndex);
        }

        public static Int32IndirectReadOnlyDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>
            Create<TKeyToIndexMap, TIndexToValueMap>(TKeyToIndexMap indexByKey, TIndexToValueMap valueByIndex)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TIndexToValueMap : IReadOnlyDictionary<int, TValue>
        {
            if (indexByKey is null)
                ThrowHelper.ThrowArgumentNullException(nameof(indexByKey));
            if (valueByIndex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(valueByIndex));
            return new(indexByKey, valueByIndex);
        }
    }
}
