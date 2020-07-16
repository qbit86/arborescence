namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;

    public struct SimpleIncidenceGraph : IIncidenceGraph<int, uint, ArraySegmentEnumerator<uint>>,
        IEquatable<SimpleIncidenceGraph>
    {
        private readonly uint[] _storage;

        private SimpleIncidenceGraph(uint[] storage)
        {
            Debug.Assert(storage != null, "storage != null");
            Debug.Assert(storage.Length > 0, "storage.Length > 0");
            Debug.Assert(unchecked((int)storage[0]) >= 0, "storage[0] >= 0");
            Debug.Assert(storage[0] <= storage.Length - 1, "storage[0] <= storage.Length - 1");

            _storage = storage;
        }

        /// <inheritdoc/>
        public bool TryGetHead(uint edge, out int head) => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool TryGetTail(uint edge, out int tail) => throw new NotImplementedException();

        /// <inheritdoc/>
        public ArraySegmentEnumerator<uint> EnumerateOutEdges(int vertex) => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool Equals(SimpleIncidenceGraph other) => _storage == other._storage;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SimpleIncidenceGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => _storage?.GetHashCode() ?? 0;

        /// <summary>
        /// Indicates whether two <see cref="SimpleIncidenceGraph"/> structures are equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="SimpleIncidenceGraph"/> structures are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(SimpleIncidenceGraph left, SimpleIncidenceGraph right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two <see cref="SimpleIncidenceGraph"/> structures are not equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the inequality operator.</param>
        /// <param name="right">The structure on the right side of the inequality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="SimpleIncidenceGraph"/> structures are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(SimpleIncidenceGraph left, SimpleIncidenceGraph right) => !left.Equals(right);
    }
}
