namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
    /// <remarks>
    /// An adjacency-list representation of a graph stores an out-edge sequence for each vertex.
    /// </remarks>
    public readonly partial struct SimpleIncidenceGraph : IIncidenceGraph<int, uint, ArraySegmentEnumerator<uint>>,
        IEquatable<SimpleIncidenceGraph>
    {
        private readonly uint[] _storage;

        private SimpleIncidenceGraph(uint[] storage)
        {
            Debug.Assert(storage != null, "storage != null");
            Debug.Assert(storage.Length > 0, "storage.Length > 0");
            Debug.Assert(storage[0] <= short.MaxValue, "storage[0] <= short.MaxValue");
            Debug.Assert(storage[0] <= storage.Length - 1, "storage[0] <= storage.Length - 1");

            _storage = storage;
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount => _storage is null ? 0 : unchecked((int)_storage[0]);

        private ReadOnlySpan<uint> UpperBoundByVertex => _storage.AsSpan(1, VertexCount);

        private uint UnsignedVertexCount => _storage?[0] ?? 0u;

        /// <inheritdoc/>
        public bool TryGetHead(uint edge, out int head)
        {
            head = unchecked((int)(edge & 0xFFFF));
            return head < VertexCount;
        }

        /// <inheritdoc/>
        public bool TryGetTail(uint edge, out int tail)
        {
            tail = unchecked((int)(edge >> 16));
            return tail < VertexCount;
        }

        /// <inheritdoc/>
        public ArraySegmentEnumerator<uint> EnumerateOutEdges(int vertex)
        {
            if (unchecked((uint)vertex) >= UnsignedVertexCount)
                return default;

            int offset = vertex == 0 ? 0 : unchecked((int)UpperBoundByVertex[vertex - 1]);
            int upperBound = unchecked((int)UpperBoundByVertex[vertex]);
            Debug.Assert(offset <= upperBound, "offset <= upperBound");
            int count = upperBound - offset;
            return new ArraySegmentEnumerator<uint>(_storage, 1 + VertexCount + offset, count);
        }

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
