namespace Arborescence
{
    using System.Collections.Generic;
    using Primitives;

    /// <summary>
    /// Provides a set of initialization methods for instances
    /// of the <see cref="Int32Dictionary{TValue, TValueList}"/>
    /// and the <see cref="Int32Dictionary{TValue, TValueList, TAbsenceComparer}"/>
    /// types.
    /// </summary>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
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
            TValueList items, TValue? absenceMarker = default)
            where TValueList : IList<TValue>
        {
            if (items is null)
                ThrowHelper.ThrowArgumentNullException(nameof(items));
            return new(items, EqualityComparer<TValue>.Default, absenceMarker);
        }

        public static Int32Dictionary<TValue, TValueList, TAbsenceComparer>
            CreateWithAbsence<TValueList, TAbsenceComparer>(
                TValueList items, TAbsenceComparer absenceComparer, TValue? absenceMarker = default)
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
