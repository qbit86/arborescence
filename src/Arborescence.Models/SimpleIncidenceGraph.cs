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
        private readonly int[] _upperBoundByVertex;
        private readonly Endpoints[] _edgesOrderedByTail;

        private SimpleIncidenceGraph(int[] upperBoundByVertex, Endpoints[] edgesOrderedByTail)
        {
            Debug.Assert(upperBoundByVertex != null, nameof(upperBoundByVertex) + " != null");
            Debug.Assert(edgesOrderedByTail != null, nameof(edgesOrderedByTail) + " != null");
            _upperBoundByVertex = upperBoundByVertex;
            _edgesOrderedByTail = edgesOrderedByTail;
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount => (_upperBoundByVertex?.Length).GetValueOrDefault();

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        public int EdgeCount => (_edgesOrderedByTail?.Length).GetValueOrDefault();

        private bool IsDefault => _upperBoundByVertex is null || _edgesOrderedByTail is null;

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
            if (IsDefault || unchecked((uint)vertex >= (uint)_upperBoundByVertex.Length))
                return ArraySegmentEnumerator<Endpoints>.Empty;

            int lowerBound = vertex == 0 ? 0 : _upperBoundByVertex[vertex - 1];
            int upperBound = _upperBoundByVertex[vertex];
            Debug.Assert(lowerBound <= upperBound, "lowerBound <= upperBound");
            return new ArraySegmentEnumerator<Endpoints>(_edgesOrderedByTail, lowerBound, upperBound);
        }

        /// <inheritdoc/>
        public bool Equals(SimpleIncidenceGraph other) =>
            _upperBoundByVertex == other._upperBoundByVertex && _edgesOrderedByTail == other._edgesOrderedByTail;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SimpleIncidenceGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => IsDefault
            ? 0
            : unchecked(_upperBoundByVertex.GetHashCode() * 397) ^ _edgesOrderedByTail.GetHashCode();

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
