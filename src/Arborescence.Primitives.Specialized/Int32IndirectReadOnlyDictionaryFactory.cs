namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using Primitives;

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
                TKey, TValue, TKeyToIndexMap, Int32ReadOnlyDictionary<TValue, TValueList, DefaultAbsencePolicy<TValue>>>
            CreateFromListWithAbsence<TKeyToIndexMap, TValueList>(
                TKeyToIndexMap indexByKey, TValueList values)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TValueList : IReadOnlyList<TValue>
        {
            if (indexByKey is null)
                ThrowHelper.ThrowArgumentNullException(nameof(indexByKey));
            if (values is null)
                ThrowHelper.ThrowArgumentNullException(nameof(values));
            Int32ReadOnlyDictionary<TValue, TValueList, DefaultAbsencePolicy<TValue>> valueByIndex =
                new(values, default);
            return new(indexByKey, valueByIndex);
        }

        public static Int32IndirectReadOnlyDictionary<
                TKey, TValue, TKeyToIndexMap, Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy>>
            CreateFromListWithAbsence<TKeyToIndexMap, TValueList, TAbsencePolicy>(
                TKeyToIndexMap indexByKey, TValueList values, TAbsencePolicy absencePolicy)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TValueList : IReadOnlyList<TValue>
            where TAbsencePolicy : IEquatable<TValue>
        {
            if (indexByKey is null)
                ThrowHelper.ThrowArgumentNullException(nameof(indexByKey));
            if (values is null)
                ThrowHelper.ThrowArgumentNullException(nameof(values));
            if (absencePolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(absencePolicy));
            Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy> valueByIndex = new(values, absencePolicy);
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
