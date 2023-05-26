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

        public static Int32ReadOnlyDictionary<TValue, TValueList, DefaultAbsence<TValue>>
            CreateWithAbsence<TValueList>(TValueList values)
            where TValueList : IReadOnlyList<TValue>
        {
            if (values is null)
                ThrowHelper.ThrowArgumentNullException(nameof(values));
            return new(values, default);
        }

        public static Int32ReadOnlyDictionary<TValue, TValueList, TAbsence>
            CreateWithAbsence<TValueList, TAbsence>(TValueList values, TAbsence absence)
            where TValueList : IReadOnlyList<TValue>
            where TAbsence : IEquatable<TValue>
        {
            if (values is null)
                ThrowHelper.ThrowArgumentNullException(nameof(values));
            if (absence is null)
                ThrowHelper.ThrowArgumentNullException(nameof(absence));
            return new(values, absence);
        }
    }
}
