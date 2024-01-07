namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides a set of initialization methods for instances
    /// of the <see cref="EqualityComparerEquatable{T,TComparer}"/> type.
    /// </summary>
    public static class EqualityComparerEquatable
    {
        /// <summary>
        /// Creates an <see cref="EqualityComparerEquatable{T,TComparer}"/>
        /// with the specified with the specified value and comparer.
        /// </summary>
        /// <param name="value">The value to capture.</param>
        /// <param name="comparer">The <typeparamref name="TComparer"/> to use when comparing.</param>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <typeparam name="TComparer">The type of the comparer to use when comparing the captured object with other objects of the same type.</typeparam>
        /// <returns>
        /// An <see cref="EqualityComparerEquatable{T,TComparer}"/> that captures the specified value and comparer.
        /// </returns>
        public static EqualityComparerEquatable<T, TComparer> Create<T, TComparer>(T value, TComparer comparer)
            where TComparer : IEqualityComparer<T> => new(value, comparer);
    }

    /// <summary>
    /// Represents a comparison operation that compares the captured object with other objects of the same type.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <typeparam name="TComparer">The type of the comparer to use when comparing the captured object with other objects of the same type.</typeparam>
    public readonly struct EqualityComparerEquatable<T, TComparer> : IEquatable<T>
        where TComparer : IEqualityComparer<T>
    {
        private readonly T _value;
        private readonly TComparer _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityComparerEquatable{T,TComparer}"/> structure
        /// with the specified value and comparer.
        /// </summary>
        /// <param name="value">The value to capture.</param>
        /// <param name="comparer">The <typeparamref name="TComparer"/> to use when comparing.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer"/> is <see langword="null"/>.
        /// </exception>
        public EqualityComparerEquatable(T value, TComparer comparer)
        {
            _value = value;
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        /// <summary>
        /// Indicates whether the captured object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with the captured object.</param>
        /// <returns>
        /// <c>true</c> if the captured object is equal to the <paramref name="other"/> parameter;
        /// otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(T other) => _comparer.Equals(_value, other);
    }
}
