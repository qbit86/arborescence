namespace Arborescence
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Models the concept of absence by treating the element as missing if it is equal to the default value of its type.
    /// </summary>
    /// <typeparam name="T">The type of the elements to check.</typeparam>
    public readonly struct DefaultEqualityComparerEquatable<T> : IEquatable<T>
    {
        /// <summary>
        /// Indicates whether the default object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with the default object.</param>
        /// <returns>
        /// <c>true</c> if the default object is equal to the <paramref name="other"/> parameter;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(T other) => EqualityComparer<T>.Default.Equals(default!, other);
    }
}
