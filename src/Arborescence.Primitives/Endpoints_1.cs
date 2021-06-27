namespace Arborescence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using Primitives;

    /// <summary>
    /// Holds endpoints of an oriented edge.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    public readonly struct Endpoints<TVertex> : IEquatable<Endpoints<TVertex>>
    {
        /// <summary>
        /// Gets the tail of the edge.
        /// </summary>
        public TVertex Tail { get; }

        /// <summary>
        /// Gets the head of the edge.
        /// </summary>
        public TVertex Head { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Endpoints{TVertex}"/> structure
        /// with the specified tail and head.
        /// </summary>
        /// <param name="tail">The tail of the edge.</param>
        /// <param name="head">The head of the edge.</param>
        public Endpoints(TVertex tail, TVertex head)
        {
            Tail = tail;
            Head = head;
        }

        private static EqualityComparer<TVertex> C => EqualityComparer<TVertex>.Default;

        /// <inheritdoc/>
        public override string ToString() => EndpointsHelpers.PairToString(Tail?.ToString(), Head?.ToString());

        /// <summary>
        /// Deconstructs the current <see cref="Endpoints{TVertex}"/> structure.
        /// </summary>
        /// <param name="tail">The tail of the edge.</param>
        /// <param name="head">The head of the edge.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out TVertex tail, out TVertex head)
        {
            tail = Tail;
            head = Head;
        }

        /// <inheritdoc/>
        public bool Equals(Endpoints<TVertex> other) => C.Equals(Tail, other.Tail) && C.Equals(Head, other.Head);

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Endpoints<TVertex> other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = Tail != null ? C.GetHashCode(Tail) : 0;
            hashCode = unchecked(hashCode * 397) ^ (Head != null ? C.GetHashCode(Head) : 0);
            return hashCode;
        }

        /// <summary>
        /// Indicates whether two <see cref="Endpoints{TVertex}"/> structures are equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="Endpoints{TVertex}"/> structures are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(Endpoints<TVertex> left, Endpoints<TVertex> right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two <see cref="Endpoints{TVertex}"/> structures are not equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the inequality operator.</param>
        /// <param name="right">The structure on the right side of the inequality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="Endpoints{TVertex}"/> structures are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(Endpoints<TVertex> left, Endpoints<TVertex> right) => !left.Equals(right);
    }
}
