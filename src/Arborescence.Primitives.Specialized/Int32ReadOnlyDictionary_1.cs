namespace Arborescence
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of initialization methods for instances of the
    /// <see cref="Int32ReadOnlyDictionary{TKey, TValueList}"/>
    /// and <see cref="Int32ReadOnlyDictionary{TKey, TValueList, TAbsence}"/>
    /// types.
    /// </summary>
    /// <typeparam name="TValue">The type of the values in this dictionary.</typeparam>
    public static class Int32ReadOnlyDictionary<TValue>
    {
        /// <summary>
        /// Creates an <see cref="Int32ReadOnlyDictionary{TValue, TValueList}"/> with the specified list of values.
        /// </summary>
        /// <param name="values">The underlying list of the values.</param>
        /// <typeparam name="TValueList">The type of the underlying list of the values.</typeparam>
        /// <returns>
        /// A <see cref="Int32ReadOnlyDictionary{TValue, TValueList}"/> that contains the specified values.
        /// </returns>
        public static Int32ReadOnlyDictionary<TValue, TValueList> Create<TValueList>(TValueList values)
            where TValueList : IReadOnlyList<TValue>
        {
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            return new(values);
        }

        /// <summary>
        /// Creates an <see cref="Int32ReadOnlyDictionary{TValue, TValueList, TAbsence}"/>
        /// where <c>TAbsence</c> is <see cref="DefaultAbsence{TValue}"/>.
        /// </summary>
        /// <param name="values">The underlying list of the values.</param>
        /// <typeparam name="TValueList">The type of the underlying list of the values.</typeparam>
        /// <returns>
        /// A <see cref="Int32ReadOnlyDictionary{TValue, TValueList, TAbsence}"/> that contains the specified values.
        /// </returns>
        public static Int32ReadOnlyDictionary<TValue, TValueList, DefaultAbsence<TValue>>
            CreateWithAbsence<TValueList>(TValueList values)
            where TValueList : IReadOnlyList<TValue>
        {
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            return new(values, default);
        }

        /// <summary>
        /// Creates an <see cref="Int32ReadOnlyDictionary{TValue, TValueList, TAbsence}"/>
        /// with the specified list of values and the absence object.
        /// </summary>
        /// <param name="values">The underlying list of the values.</param>
        /// <param name="absenceEquatable">The object that provides a method for distinguishing missing elements.</param>
        /// <typeparam name="TValueList">The type of the underlying list of the values.</typeparam>
        /// <typeparam name="TEquatable">The type that provides a method for distinguishing missing elements.</typeparam>
        /// <returns>
        /// A <see cref="Int32ReadOnlyDictionary{TValue, TValueList, TAbsence}"/> that contains the specified values.
        /// </returns>
        public static Int32ReadOnlyDictionary<TValue, TValueList, TEquatable>
            CreateWithAbsence<TValueList, TEquatable>(TValueList values, TEquatable absenceEquatable)
            where TValueList : IReadOnlyList<TValue>
            where TEquatable : IEquatable<TValue>
        {
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            if (absenceEquatable is null)
                ArgumentNullExceptionHelpers.Throw(nameof(absenceEquatable));
            return new(values, absenceEquatable);
        }

        public static Int32ReadOnlyDictionary<TValue, TValueList, ComparerEquatable<TValue, TComparer>>
            CreateWithAbsence<TValueList, TComparer>(TValueList values, TValue absence, TComparer absenceComparer)
            where TValueList : IReadOnlyList<TValue>
            where TComparer : IEqualityComparer<TValue>
        {
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            if (absence is null)
                ArgumentNullExceptionHelpers.Throw(nameof(absence));
            if (absenceComparer is null)
                ArgumentNullExceptionHelpers.Throw(nameof(absenceComparer));
            ComparerEquatable<TValue, TComparer> comparerEquatable = new(absence, absenceComparer);
            return new(values, comparerEquatable);
        }
    }
}
