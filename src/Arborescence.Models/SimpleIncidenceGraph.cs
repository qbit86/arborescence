namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

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
        // 1    | m — the number of edges
        // n    | upper bounds of out-edge enumerators indexed by vertices
        private readonly int[] _data;
        private readonly Endpoints[] _edgesOrderedByTail;

        private SimpleIncidenceGraph(int[] data, Endpoints[] edgesOrderedByTail)
        {
            Debug.Assert(edgesOrderedByTail != null, nameof(edgesOrderedByTail) + " != null");
            Debug.Assert(data != null, nameof(data) + " != null");
            Debug.Assert(data.Length >= 2, "data.Length >= 2");
            Debug.Assert(data[0] >= 0, "data[0] >= 0");
            Debug.Assert(data[0] <= data.Length - 2, "data[0] <= data.Length - 2");
            Debug.Assert(data[1] >= 0, "data[1] >= 0");
            Debug.Assert(data[1] <= edgesOrderedByTail.Length, "data[1] <= edgesOrderedByTail.Length");

            _data = data;
            _edgesOrderedByTail = edgesOrderedByTail;
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount => (_data?[0]).GetValueOrDefault();

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        public int EdgeCount => (_data?[1]).GetValueOrDefault();

        private bool IsDefault => _data is null || _edgesOrderedByTail is null;

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
            ReadOnlySpan<int> upperBoundByVertex = GetUpperBoundByVertex();
            if (IsDefault || unchecked((uint)vertex >= (uint)upperBoundByVertex.Length))
                return ArraySegmentEnumerator<Endpoints>.Empty;

            int lowerBound = vertex == 0 ? 0 : upperBoundByVertex[vertex - 1];
            int upperBound = upperBoundByVertex[vertex];
            Debug.Assert(lowerBound <= upperBound, "lowerBound <= upperBound");
            return new ArraySegmentEnumerator<Endpoints>(_edgesOrderedByTail, lowerBound, upperBound);
        }

        /// <inheritdoc/>
        public bool Equals(SimpleIncidenceGraph other) =>
            _data == other._data && _edgesOrderedByTail == other._edgesOrderedByTail;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SimpleIncidenceGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => IsDefault
            ? 0
            : unchecked(_data.GetHashCode() * 397) ^ _edgesOrderedByTail.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetUpperBoundByVertex() =>
            IsDefault ? ReadOnlySpan<int>.Empty : _data.AsSpan(2, VertexCount);

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
