namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
    public readonly partial struct IndexedIncidenceGraph : IIncidenceGraph<int, int, ArraySegmentEnumerator<int>>,
        IEquatable<IndexedIncidenceGraph>
    {
        // Layout:
        // 1    | n — the number of vertices
        // n    | upper bounds of out-edge enumerators indexed by vertices
        // m    | edges sorted by tail
        // m    | heads indexed by edges
        // m    | tails indexed by edges
        private readonly int[] _data;

        internal IndexedIncidenceGraph(int[] data)
        {
            Debug.Assert(data != null, nameof(data) + " != null");
            Debug.Assert(data.Length > 0, "data.Length > 0");
            Debug.Assert(data[0] >= 0, "data[0] >= 0");
            Debug.Assert(data[0] <= data.Length - 1, "data[0] <= data.Length - 1");

            _data = data;
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount => (_data?[0]).GetValueOrDefault();

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        public int EdgeCount => _data is null ? 0 : (_data.Length - 1 - VertexCount) / 3;

        /// <inheritdoc/>
        public bool TryGetTail(int edge, out int tail)
        {
            ReadOnlySpan<int> tailByEdge = GetTailByEdge();
            if (unchecked((uint)edge >= (uint)tailByEdge.Length))
            {
                tail = default;
                return false;
            }

            tail = tailByEdge[edge];
            return true;
        }

        /// <inheritdoc/>
        public bool TryGetHead(int edge, out int head)
        {
            ReadOnlySpan<int> headByEdge = GetHeadByEdge();
            if (unchecked((uint)edge >= (uint)headByEdge.Length))
            {
                head = default;
                return false;
            }

            head = headByEdge[edge];
            return true;
        }

        /// <inheritdoc/>
        public ArraySegmentEnumerator<int> EnumerateOutEdges(int vertex)
        {
            ReadOnlySpan<int> upperBoundByVertex = GetUpperBoundByVertex();
            if (unchecked((uint)vertex >= (uint)upperBoundByVertex.Length))
                return new ArraySegmentEnumerator<int>(Array.Empty<int>(), 0, 0);

            int lowerBound = vertex == 0 ? 0 : upperBoundByVertex[vertex - 1];
            int upperBound = upperBoundByVertex[vertex];
            Debug.Assert(lowerBound <= upperBound, "lowerBound <= upperBound");
            int offset = 1 + VertexCount;
            return new ArraySegmentEnumerator<int>(_data, offset + lowerBound, offset + upperBound);
        }

        /// <inheritdoc/>
        public bool Equals(IndexedIncidenceGraph other) => _data == other._data;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is IndexedIncidenceGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => (_data?.GetHashCode()).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetUpperBoundByVertex() => _data.AsSpan(1, VertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetEdgesSortedByTail() => _data.AsSpan(1 + VertexCount, EdgeCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTailByEdge() => _data.AsSpan(1 + VertexCount + EdgeCount + EdgeCount, EdgeCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetHeadByEdge() => _data.AsSpan(1 + VertexCount + EdgeCount, EdgeCount);

        /// <summary>
        /// Indicates whether two <see cref="IndexedIncidenceGraph"/> structures are equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="IndexedIncidenceGraph"/> structures are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(IndexedIncidenceGraph left, IndexedIncidenceGraph right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two <see cref="IndexedIncidenceGraph"/> structures are not equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the inequality operator.</param>
        /// <param name="right">The structure on the right side of the inequality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="IndexedIncidenceGraph"/> structures are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(IndexedIncidenceGraph left, IndexedIncidenceGraph right) => !left.Equals(right);
    }
}
