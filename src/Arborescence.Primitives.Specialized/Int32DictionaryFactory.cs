namespace Arborescence
{
    using System.Collections.Generic;
    using Primitives;

    public static class Int32DictionaryFactory<TValue>
    {
        public static Int32Dictionary<TValue, TValueList> Create<TValueList>(TValueList items)
            where TValueList : IList<TValue>
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(nameof(items));
            return new(items);
        }

        public static Int32Dictionary<TValue, TValueList, EqualityComparer<TValue>> CreateWithAbsence<TValueList>(
            TValueList items)
            where TValueList : IList<TValue>
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(nameof(items));
            return new(items, EqualityComparer<TValue>.Default, default);
        }

        public static Int32Dictionary<TValue, TValueList, EqualityComparer<TValue>> CreateWithAbsence<TValueList>(
            TValueList items, TValue? absenceMarker)
            where TValueList : IList<TValue>
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(nameof(items));
            return new(items, EqualityComparer<TValue>.Default, absenceMarker);
        }

        public static Int32Dictionary<TValue, TValueList, TAbsenceComparer>
            CreateWithAbsence<TValueList, TAbsenceComparer>(TValueList items, TAbsenceComparer absenceComparer)
            where TValueList : IList<TValue>
            where TAbsenceComparer : IEqualityComparer<TValue>
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(nameof(items));
            if (absenceComparer is null)
                ThrowHelper.ThrowArgumentNullException(nameof(absenceComparer));
            return new(items, absenceComparer, default);
        }

        public static Int32Dictionary<TValue, TValueList, TAbsenceComparer>
            CreateWithAbsence<TValueList, TAbsenceComparer>(
                TValueList items, TAbsenceComparer absenceComparer, TValue? absenceMarker)
            where TValueList : IList<TValue>
            where TAbsenceComparer : IEqualityComparer<TValue>
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(nameof(items));
            if (absenceComparer is null)
                ThrowHelper.ThrowArgumentNullException(nameof(absenceComparer));
            return new(items, absenceComparer, absenceMarker);
        }
    }
}
