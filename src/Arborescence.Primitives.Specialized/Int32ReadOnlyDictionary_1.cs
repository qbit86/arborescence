namespace Arborescence
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of initialization methods for instances of the
    /// <see cref="Int32ReadOnlyDictionary{TKey, TValueList}"/>
    /// and <see cref="Int32ReadOnlyDictionary{TKey, TValueList, TEquatable}"/>
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
        /// Creates an <see cref="Int32ReadOnlyDictionary{TValue, TValueList, TEquatable}"/>
        /// where <c>TEquatable</c> is <see cref="DefaultEqualityComparerEquatable{T}"/>.
        /// </summary>
        /// <param name="values">The underlying list of the values.</param>
        /// <typeparam name="TValueList">The type of the underlying list of the values.</typeparam>
        /// <returns>
        /// A <see cref="Int32ReadOnlyDictionary{TValue, TValueList, TEquatable}"/> that contains the specified values.
        /// </returns>
        public static Int32ReadOnlyDictionary<TValue, TValueList, DefaultEqualityComparerEquatable<TValue?>>
            CreateWithAbsence<TValueList>(TValueList values)
            where TValueList : IReadOnlyList<TValue>
        {
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            DefaultEqualityComparerEquatable<TValue?> absenceEquatable = default;
            return new(values, absenceEquatable);
        }

        public static Int32ReadOnlyDictionary<TValue, TValueList, EqualityComparerEquatable<TValue?, TComparer>>
            CreateWithAbsence<TValueList, TComparer>(TValueList values, TValue? absence, TComparer absenceComparer)
            where TValueList : IReadOnlyList<TValue>
            where TComparer : IEqualityComparer<TValue?>
        {
            if (values is null)
                ArgumentNullExceptionHelpers.Throw(nameof(values));
            if (absenceComparer is null)
                ArgumentNullExceptionHelpers.Throw(nameof(absenceComparer));
            EqualityComparerEquatable<TValue?, TComparer> absenceEquatable = new(absence, absenceComparer);
            return new(values, absenceEquatable);
        }

        /// <summary>
        /// Creates an <see cref="Int32ReadOnlyDictionary{TValue, TValueList, TEquatable}"/>
        /// with the specified list of values and the absence object.
        /// </summary>
        /// <param name="values">The underlying list of the values.</param>
        /// <param name="absenceEquatable">The object that provides a method for distinguishing missing elements.</param>
        /// <typeparam name="TValueList">The type of the underlying list of the values.</typeparam>
        /// <typeparam name="TEquatable">The type that provides a method for distinguishing missing elements.</typeparam>
        /// <returns>
        /// A <see cref="Int32ReadOnlyDictionary{TValue, TValueList, TEquatable}"/> that contains the specified values.
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
    }
}
