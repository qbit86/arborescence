namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
    /// <remarks>
    /// An adjacency-list representation of a graph stores an out-edge sequence for each vertex.
    /// </remarks>
    public readonly partial struct SimpleIncidenceGraph :
        IIncidenceGraph<int, Endpoints, ArraySegmentEnumerator<Endpoints>>,
        IEquatable<SimpleIncidenceGraph>
    {
        // Layout:
        // 1    | n — the number of vertices
        // n    | upper bounds of out-edge enumerators
        // m    | edges sorted by tail
        private readonly uint[] _storage;

        private SimpleIncidenceGraph(uint[] storage)
        {
            Debug.Assert(storage != null, "storage != null");
            Debug.Assert(storage.Length > 0, "storage.Length > 0");
            Debug.Assert(storage[0] <= 0x10000u, "storage[0] <= 0x10000u");
            Debug.Assert(storage[0] <= storage.Length - 1, "storage[0] <= storage.Length - 1");

            _storage = storage;
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount => _storage is null ? 0 : (int)_storage[0];

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        public int EdgeCount
        {
            get
            {
                if (VertexCount == 0)
                    return 0;

                int edgeCount = _storage.Length - 1 - VertexCount;
                Debug.Assert(edgeCount == (int)UpperBoundByVertex[VertexCount - 1]);
                return edgeCount;
            }
        }

        private ReadOnlySpan<uint> UpperBoundByVertex => _storage.AsSpan(1, VertexCount);

        /// <inheritdoc/>
        public bool TryGetHead(Endpoints edge, out int head)
        {
            head = edge.Head;
            return head < VertexCount;
        }

        /// <inheritdoc/>
        public bool TryGetTail(Endpoints edge, out int tail)
        {
            tail = edge.Tail;
            return tail < VertexCount;
        }

        /// <inheritdoc/>
        public ArraySegmentEnumerator<Endpoints> EnumerateOutEdges(int vertex)
        {
#if false
            ReadOnlySpan<uint> upperBoundByVertex = UpperBoundByVertex;
            if (unchecked((uint)vertex >= (uint)upperBoundByVertex.Length))
                return new ArraySegmentEnumerator<uint>(Array.Empty<uint>(), 0, 0);

            int lowerBound = vertex == 0 ? 0 : (int)UpperBoundByVertex[vertex - 1];
            int upperBound = (int)UpperBoundByVertex[vertex];
            Debug.Assert(lowerBound <= upperBound, "lowerBound <= upperBound");
            int offset = 1 + VertexCount;
            return new ArraySegmentEnumerator<uint>(_storage, offset + lowerBound, offset + upperBound);
#else
            throw new NotImplementedException();
#endif
        }

        /// <inheritdoc/>
        public bool Equals(SimpleIncidenceGraph other) => _storage == other._storage;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SimpleIncidenceGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => (_storage?.GetHashCode()).GetValueOrDefault();

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
