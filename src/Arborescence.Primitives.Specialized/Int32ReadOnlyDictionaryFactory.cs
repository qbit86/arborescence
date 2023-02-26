namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using Primitives;

    public static class Int32ReadOnlyDictionaryFactory<TValue>
    {
        public static Int32ReadOnlyDictionary<TValue, TValueList> Create<TValueList>(TValueList items)
            where TValueList : IReadOnlyList<TValue>
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(nameof(items));
            return new(items);
        }

        public static Int32ReadOnlyDictionary<TValue, TValueList, DefaultAbsencePolicy<TValue>>
            CreateWithAbsencePolicy<TValueList>(TValueList items)
            where TValueList : IReadOnlyList<TValue>
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(nameof(items));
            return new(items, default);
        }

        public static Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy>
            CreateWithAbsencePolicy<TValueList, TAbsencePolicy>(TValueList items, TAbsencePolicy absencePolicy)
            where TValueList : IReadOnlyList<TValue>
            where TAbsencePolicy : IEquatable<TValue>
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(nameof(items));
            if (absencePolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(absencePolicy));
            return new(items, absencePolicy);
        }
    }
}
