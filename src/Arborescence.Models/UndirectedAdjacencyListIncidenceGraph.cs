namespace Arborescence.Models
{
    using System;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
    public readonly struct UndirectedAdjacencyListIncidenceGraph :
        IIncidenceGraph<int, int, ArraySegmentEnumerator<int>>, IEquatable<UndirectedAdjacencyListIncidenceGraph>
    {
        private readonly int[] _storage;

        internal UndirectedAdjacencyListIncidenceGraph(int[] storage)
        {
            Assert(storage != null, "storage != null");
            Assert(storage.Length > 0, "storage.Length > 0");

            Assert(storage[0] >= 0, "storage[0] >= 0");
            Assert(storage[0] <= storage.Length - 1, "storage[0] <= storage.Length - 1");

            _storage = storage;
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount => _storage == null ? 0 : GetVertexCount();

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        public int EdgeCount => _storage == null ? 0 : (_storage.Length - 1 - 2 * GetVertexCount()) / 4;

        /// <inheritdoc/>
        public bool TryGetTail(int edge, out int tail)
        {
            int actualEdge = edge < 0 ? ~edge : edge;
            ReadOnlySpan<int> actualTails = edge < 0 ? GetHeads() : GetTails();
            if (actualEdge >= actualTails.Length)
            {
                tail = default;
                return false;
            }

            tail = actualTails[actualEdge];
            return true;
        }

        /// <inheritdoc/>
        public bool TryGetHead(int edge, out int head)
        {
            int actualEdge = edge < 0 ? ~edge : edge;
            ReadOnlySpan<int> actualHeads = edge < 0 ? GetTails() : GetHeads();
            if (actualEdge >= actualHeads.Length)
            {
                head = default;
                return false;
            }

            head = actualHeads[actualEdge];
            return true;
        }

        /// <inheritdoc/>
        public ArraySegmentEnumerator<int> EnumerateOutEdges(int vertex)
        {
            ReadOnlySpan<int> edgeBounds = GetEdgeBounds();

            if ((uint)(2 * vertex) >= (uint)edgeBounds.Length)
                return new ArraySegmentEnumerator<int>(ArrayBuilder<int>.EmptyArray, 0, 0);

            int start = edgeBounds[2 * vertex];
            int length = edgeBounds[2 * vertex + 1];
            Assert(length >= 0, "length >= 0");

            return new ArraySegmentEnumerator<int>(_storage, start, length);
        }

        /// <inheritdoc/>
        public bool Equals(UndirectedAdjacencyListIncidenceGraph other) => _storage == other._storage;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is UndirectedAdjacencyListIncidenceGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => _storage?.GetHashCode() ?? 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetEdgeBounds() => _storage.AsSpan(1, 2 * VertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTails() => _storage.AsSpan(1 + 2 * VertexCount + 3 * EdgeCount, EdgeCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetHeads() => _storage.AsSpan(1 + 2 * VertexCount + 2 * EdgeCount, EdgeCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetVertexCount()
        {
            Assert(_storage != null, "_storage != null");
            Assert(_storage.Length > 0, "_storage.Length > 0");

            int result = _storage[0];
            Assert(result >= 0, "result >= 0");
            Assert(2 * result <= _storage.Length - 1, "2 * result <= _storage.Length - 1");

            return result;
        }

        /// <summary>
        /// Indicates whether two <see cref="UndirectedAdjacencyListIncidenceGraph"/> structures are equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="UndirectedAdjacencyListIncidenceGraph"/> structures are equal;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(UndirectedAdjacencyListIncidenceGraph left,
            UndirectedAdjacencyListIncidenceGraph right) => left.Equals(right);

        /// <summary>
        /// Indicates whether two <see cref="UndirectedAdjacencyListIncidenceGraph"/> structures are not equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the inequality operator.</param>
        /// <param name="right">The structure on the right side of the inequality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="UndirectedAdjacencyListIncidenceGraph"/> structures are not equal;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(UndirectedAdjacencyListIncidenceGraph left,
            UndirectedAdjacencyListIncidenceGraph right) => !left.Equals(right);
    }
}
