namespace Arborescence.Models
{
    using System;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
    public readonly struct EdgeListIncidenceGraph :
        IIncidenceGraph<int, Endpoints<int>, ArraySegmentEnumerator<Endpoints<int>>>,
        IEquatable<EdgeListIncidenceGraph>
    {
        private readonly Endpoints<int>[] _storage;

        internal EdgeListIncidenceGraph(int vertexCount, Endpoints<int>[] storage)
        {
            Assert(vertexCount >= 0, "vertexCount >= 0");
            Assert(storage != null, "storage != null");

            _storage = storage;
            VertexCount = vertexCount;
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount { get; }

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        public int EdgeCount => _storage?.Length - VertexCount ?? 0;

        /// <inheritdoc/>
        public bool TryGetTail(Endpoints<int> edge, out int tail)
        {
            tail = edge.Tail;
            return (uint)edge.Tail < (uint)VertexCount;
        }

        /// <inheritdoc/>
        public bool TryGetHead(Endpoints<int> edge, out int head)
        {
            head = edge.Head;
            return (uint)edge.Head < (uint)VertexCount;
        }

        /// <inheritdoc/>
        public ArraySegmentEnumerator<Endpoints<int>> EnumerateOutEdges(int vertex)
        {
            ReadOnlySpan<Endpoints<int>> edgeBounds = GetEdgeBounds();
            if ((uint)vertex >= (uint)edgeBounds.Length)
            {
                return new ArraySegmentEnumerator<Endpoints<int>>(
                    ArrayBuilder<Endpoints<int>>.EmptyArray, 0, 0);
            }

            int start = edgeBounds[vertex].Tail;
            int length = edgeBounds[vertex].Head;
            return new ArraySegmentEnumerator<Endpoints<int>>(_storage, start, length);
        }

        /// <inheritdoc/>
        public bool Equals(EdgeListIncidenceGraph other) =>
            VertexCount == other.VertexCount && _storage == other._storage;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is EdgeListIncidenceGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => unchecked(VertexCount * 397) ^ (_storage?.GetHashCode() ?? 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<Endpoints<int>> GetEdgeBounds() => _storage.AsSpan(EdgeCount, VertexCount);

        /// <summary>
        /// Indicates whether two <see cref="EdgeListIncidenceGraph"/> structures are equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the equality operator.</param>
        /// <param name="right">The structure on the right side of the equality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="EdgeListIncidenceGraph"/> structures are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(EdgeListIncidenceGraph left, EdgeListIncidenceGraph right) =>
            left.Equals(right);

        /// <summary>
        /// Indicates whether two <see cref="EdgeListIncidenceGraph"/> structures are not equal.
        /// </summary>
        /// <param name="left">The structure on the left side of the inequality operator.</param>
        /// <param name="right">The structure on the right side of the inequality operator.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="EdgeListIncidenceGraph"/> structures are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(EdgeListIncidenceGraph left, EdgeListIncidenceGraph right) =>
            !left.Equals(right);
    }
}
