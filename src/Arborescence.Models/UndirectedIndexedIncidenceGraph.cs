namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
    public readonly partial struct UndirectedIndexedIncidenceGraph :
        IIncidenceGraph<int, int, ArraySegmentEnumerator<int>>, IEquatable<UndirectedIndexedIncidenceGraph>
    {
        // Layout:
        // 1    | n — the number of vertices
        // 1    | m — the number of edges
        // n    | upper bounds of out-edge enumerators indexed by vertices
        // 2×m  | edges sorted by tail
        // m    | heads indexed by edges
        // m    | tails indexed by edges
        private readonly int[] _data;

        internal UndirectedIndexedIncidenceGraph(int[] data)
        {
            Debug.Assert(data != null, nameof(data) + " != null");
            Debug.Assert(data.Length > 0, "data.Length > 0");
            Debug.Assert(data[0] >= 0, "data[0] >= 0");
            Debug.Assert(data[0] <= data.Length - 2, "data[0] <= data.Length - 2");
            Debug.Assert(data[1] >= 0, "data[1] >= 0");
            Debug.Assert(data[1] <= data.Length - 2, "data[1] <= data.Length - 2");

            _data = data;
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount => (_data?[0]).GetValueOrDefault();

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        public int EdgeCount => (_data?[1]).GetValueOrDefault();

        private bool IsDefault => _data is null;

        /// <inheritdoc/>
        public bool TryGetTail(int edge, out int tail)
        {
            int edgeIndex = edge < 0 ? ~edge : edge;
            ReadOnlySpan<int> endpointByEdge = edge < 0 ? GetHeadByEdge() : GetTailByEdge();
            if (edgeIndex >= endpointByEdge.Length)
            {
                tail = default;
                return false;
            }

            tail = endpointByEdge[edgeIndex];
            return true;
        }

        /// <inheritdoc/>
        public bool TryGetHead(int edge, out int head)
        {
            int edgeIndex = edge < 0 ? ~edge : edge;
            ReadOnlySpan<int> endpointByEdge = edge < 0 ? GetTailByEdge() : GetHeadByEdge();
            if (edgeIndex >= endpointByEdge.Length)
            {
                head = default;
                return false;
            }

            head = endpointByEdge[edgeIndex];
            return true;
        }

        /// <inheritdoc/>
        public ArraySegmentEnumerator<int> EnumerateOutEdges(int vertex)
        {
            ReadOnlySpan<int> upperBoundByVertex = GetUpperBoundByVertex();
            if (unchecked((uint)vertex >= (uint)upperBoundByVertex.Length))
                return ArraySegmentEnumerator<int>.Empty;

            int lowerBound = vertex == 0 ? 0 : upperBoundByVertex[vertex - 1];
            int upperBound = upperBoundByVertex[vertex];
            Debug.Assert(lowerBound <= upperBound, "lowerBound <= upperBound");
            int offset = 2 + VertexCount;
            return new ArraySegmentEnumerator<int>(_data, offset + lowerBound, offset + upperBound);
        }

        /// <inheritdoc/>
        public bool Equals(UndirectedIndexedIncidenceGraph other) => _data == other._data;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is UndirectedIndexedIncidenceGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => (_data?.GetHashCode()).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetUpperBoundByVertex() =>
            IsDefault ? ReadOnlySpan<int>.Empty : _data.AsSpan(2, VertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTailByEdge() => IsDefault
            ? ReadOnlySpan<int>.Empty
            : _data.AsSpan(2 + VertexCount + EdgeCount + EdgeCount + EdgeCount, EdgeCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetHeadByEdge() =>
            IsDefault ? ReadOnlySpan<int>.Empty : _data.AsSpan(2 + VertexCount + EdgeCount + EdgeCount, EdgeCount);

        /// <summary>
        /// Indicates whether two <see cref="UndirectedIndexedIncidenceGraph"/> structures are equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="UndirectedIndexedIncidenceGraph"/> structures are equal;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(UndirectedIndexedIncidenceGraph left,
            UndirectedIndexedIncidenceGraph right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two <see cref="UndirectedIndexedIncidenceGraph"/> structures are not equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the inequality operator.</param>
        /// <param name="right">The structure on the right side of the inequality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="UndirectedIndexedIncidenceGraph"/> structures are not equal;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(UndirectedIndexedIncidenceGraph left,
            UndirectedIndexedIncidenceGraph right) => !left.Equals(right);
    }
}
