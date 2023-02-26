namespace Arborescence
{
    using System.Collections.Generic;
    using Primitives;

    public static class Int32IndirectDictionaryFactory<TKey, TValue>
    {
        public static Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, Int32Dictionary<TValue, TValueList>>
            CreateFromList<TKeyToIndexMap, TValueList>(TKeyToIndexMap indexByKey, TValueList values)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TValueList : IList<TValue>
        {
            if (indexByKey is null)
                ThrowHelper.ThrowArgumentNullException(nameof(indexByKey));
            if (values is null)
                ThrowHelper.ThrowArgumentNullException(nameof(values));
            Int32Dictionary<TValue, TValueList> valueByIndex = new(values);
            return new(indexByKey, valueByIndex);
        }

        public static Int32IndirectDictionary<
                TKey, TValue, TKeyToIndexMap, Int32Dictionary<TValue, TValueList, EqualityComparer<TValue>>>
            CreateFromListWithAbsence<TKeyToIndexMap, TValueList>(
                TKeyToIndexMap indexByKey, TValueList values, TValue? absenceMarker = default)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TValueList : IList<TValue>
        {
            if (indexByKey is null)
                ThrowHelper.ThrowArgumentNullException(nameof(indexByKey));
            if (values is null)
                ThrowHelper.ThrowArgumentNullException(nameof(values));
            Int32Dictionary<TValue, TValueList, EqualityComparer<TValue>> valueByIndex =
                new(values, EqualityComparer<TValue>.Default, absenceMarker);
            return new(indexByKey, valueByIndex);
        }

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
                ThrowHelper.ThrowArgumentNullException(nameof(indexByKey));
            if (values is null)
                ThrowHelper.ThrowArgumentNullException(nameof(values));
            if (absenceComparer is null)
                ThrowHelper.ThrowArgumentNullException(nameof(absenceComparer));
            Int32Dictionary<TValue, TValueList, TAbsenceComparer> valueByIndex =
                new(values, absenceComparer, absenceMarker);
            return new(indexByKey, valueByIndex);
        }

        public static Int32IndirectDictionary<TKey, TValue, TKeyToIndexMap, TIndexToValueMap>
            Create<TKeyToIndexMap, TIndexToValueMap>(TKeyToIndexMap indexByKey, TIndexToValueMap valueByIndex)
            where TKeyToIndexMap : IReadOnlyDictionary<TKey, int>
            where TIndexToValueMap : IDictionary<int, TValue>
        {
            if (indexByKey is null)
                ThrowHelper.ThrowArgumentNullException(nameof(indexByKey));
            if (valueByIndex is null)
                ThrowHelper.ThrowArgumentNullException(nameof(valueByIndex));
            return new(indexByKey, valueByIndex);
        }
    }
}
