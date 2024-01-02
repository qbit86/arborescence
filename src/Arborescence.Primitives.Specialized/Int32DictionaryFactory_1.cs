namespace Arborescence
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of initialization methods for instances
    /// of the <see cref="Int32Dictionary{TValue, TValueList}"/>
    /// and the <see cref="Int32Dictionary{TValue, TValueList, TAbsenceComparer}"/>
    /// types.
    /// </summary>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public static class Int32DictionaryFactory<TValue>
    {
        /// <summary>
        /// Creates an <see cref="Int32Dictionary{TValue, TValueList}"/> with the specified list of values.
        /// </summary>
        /// <param name="values">The underlying list of the values.</param>
        /// <typeparam name="TValueList">The type of the backing list.</typeparam>
        /// <returns>
        /// A <see cref="Int32Dictionary{TValue, TValueList}"/> that contains the specified values.
        /// </returns>
        public static Int32Dictionary<TValue, TValueList> Create<TValueList>(TValueList values)
            where TValueList : IList<TValue>
        {
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            return new(values);
        }

        /// <summary>
        /// Creates an <see cref="Int32Dictionary{TValue, TValueList}"/> with the specified list of values
        /// and the absence marker to compare values against with the default comparer.
        /// </summary>
        /// <param name="values">The underlying list of the values.</param>
        /// <param name="absenceMarker">The object to use as the absence marker.</param>
        /// <typeparam name="TValueList">The type of the backing list.</typeparam>
        /// <returns>
        /// A <see cref="Int32Dictionary{TValue, TValueList, TAbsenceComparer}"/> that contains the specified values.
        /// </returns>
        public static Int32Dictionary<TValue, TValueList, EqualityComparer<TValue>> CreateWithAbsence<TValueList>(
            TValueList values, TValue? absenceMarker = default)
            where TValueList : IList<TValue>
        {
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            return new(values, EqualityComparer<TValue>.Default, absenceMarker);
        }

        /// <summary>
        /// Creates an <see cref="Int32Dictionary{TValue, TValueList}"/> with the specified list of values
        /// and the absence marker to compare values against with the specified comparer.
        /// </summary>
        /// <param name="values">The underlying list of the values.</param>
        /// <param name="absenceComparer">The comparer to compare the values with the absence marker.</param>
        /// <param name="absenceMarker">The object to use as the absence marker.</param>
        /// <typeparam name="TValueList">The type of the backing list.</typeparam>
        /// <typeparam name="TAbsenceComparer">The type that provides a method to distinguish missing elements.</typeparam>
        /// <returns>
        /// A <see cref="Int32Dictionary{TValue, TValueList, TAbsenceComparer}"/> that contains the specified values.
        /// </returns>
        public static Int32Dictionary<TValue, TValueList, TAbsenceComparer>
            CreateWithAbsence<TValueList, TAbsenceComparer>(
                TValueList values, TAbsenceComparer absenceComparer, TValue? absenceMarker = default)
            where TValueList : IList<TValue>
            where TAbsenceComparer : IEqualityComparer<TValue>
        {
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            if (absenceComparer is null)
                ArgumentNullExceptionHelpers.Throw(nameof(absenceComparer));
            return new(values, absenceComparer, absenceMarker);
        }
    }
}
