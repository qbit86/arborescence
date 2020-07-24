namespace Arborescence
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Holds endpoints of an oriented edge.
    /// </summary>
    public readonly struct Endpoints : IEquatable<Endpoints>
    {
        private readonly ulong _data;

        internal Endpoints(long data)
        {
            _data = unchecked((ulong)data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Endpoints"/> structure
        /// with the specified tail and head.
        /// </summary>
        /// <param name="tail">The tail of the edge.</param>
        /// <param name="head">The head of the edge.</param>
        public Endpoints(int tail, int head)
        {
            _data = unchecked(((ulong)tail << 32) | (uint)head);
        }

        /// <summary>
        /// Gets the tail of the edge.
        /// </summary>
        public int Tail => unchecked((int)(_data >> 32));

        /// <summary>
        /// Gets the head of the edge.
        /// </summary>
        public int Head => unchecked((int)_data);

        internal long Data => unchecked((long)_data);

        /// <inheritdoc/>
        public override string ToString()
        {
            // Consider using int.TryFormat() for netstandard2.1.
            CultureInfo f = CultureInfo.InvariantCulture;
            return EndpointsHelper.PairToString(Tail.ToString(f), Head.ToString(f));
        }

        /// <summary>
        /// Deconstructs the current <see cref="Endpoints"/> structure.
        /// </summary>
        /// <param name="tail">The tail of the edge.</param>
        /// <param name="head">The head of the edge.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out int tail, out int head)
        {
            tail = Tail;
            head = Head;
        }

        /// <summary>
        /// Creates a new <see cref="Endpoints"/> from the given values.
        /// </summary>
        /// <param name="tail">The tail of the edge.</param>
        /// <param name="head">The head of the edge.</param>
        /// <returns>A new instance of the <see cref="Endpoints"/> structure.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Endpoints Create(int tail, int head)
        {
            return new Endpoints(tail, head);
        }

        /// <inheritdoc/>
        public bool Equals(Endpoints other) => _data == other._data;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Endpoints other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => _data.GetHashCode();

        /// <summary>
        /// Indicates whether two <see cref="Endpoints"/> structures are equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="Endpoints"/> structures are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(Endpoints left, Endpoints right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether two <see cref="Endpoints"/> structures are not equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the inequality operator.</param>
        /// <param name="right">The structure on the right side of the inequality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="Endpoints"/> structures are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(Endpoints left, Endpoints right)
        {
            return !left.Equals(right);
        }
    }
}
