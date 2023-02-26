namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using Primitives;

    public static class Int32ReadOnlyDictionaryFactory<TValue>
    {
        public static Int32ReadOnlyDictionary<TValue, TValueList> Create<TValueList>(TValueList values)
            where TValueList : IReadOnlyList<TValue>
        {
            if (values is null)
                ThrowHelper.ThrowArgumentNullException(nameof(values));
            return new(values);
        }

        public static Int32ReadOnlyDictionary<TValue, TValueList, DefaultAbsencePolicy<TValue>>
            CreateWithAbsence<TValueList>(TValueList values)
            where TValueList : IReadOnlyList<TValue>
        {
            if (values is null)
                ThrowHelper.ThrowArgumentNullException(nameof(values));
            return new(values, default);
        }

        public static Int32ReadOnlyDictionary<TValue, TValueList, TAbsencePolicy>
            CreateWithAbsence<TValueList, TAbsencePolicy>(TValueList values, TAbsencePolicy absencePolicy)
            where TValueList : IReadOnlyList<TValue>
            where TAbsencePolicy : IEquatable<TValue>
        {
            if (values is null)
                ThrowHelper.ThrowArgumentNullException(nameof(values));
            if (absencePolicy is null)
                ThrowHelper.ThrowArgumentNullException(nameof(absencePolicy));
            return new(values, absencePolicy);
        }
    }
}
