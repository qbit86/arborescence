﻿#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    /// <inheritdoc cref="Arborescence.IIncidenceGraph{TVertex, TEdge, TEdges}"/>
    /// <remarks>
    /// An adjacency-list representation of a graph stores an out-edge sequence for each vertex.
    /// </remarks>
    public readonly partial struct SimpleIncidenceGraph :
        IIncidenceGraph<int, Endpoints, ArraySegment<Endpoints>.Enumerator>,
        IEquatable<SimpleIncidenceGraph>
    {
        // Layout:
        // 1    | n — the number of vertices
        // 1    | m — the number of edges
        // n    | upper bounds of out-edge enumerators indexed by vertices
        private readonly int[]? _data;
        private readonly Endpoints[]? _edgesOrderedByTail;

        internal SimpleIncidenceGraph(int[] data, Endpoints[] edgesOrderedByTail)
        {
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

        /// <inheritdoc/>
        public bool TryGetHead(Endpoints edge, out int head)
        {
            head = edge.Head;
            return unchecked((uint)head < (uint)VertexCount);
        }

        /// <inheritdoc/>
        public bool TryGetTail(Endpoints edge, out int tail)
        {
            tail = edge.Tail;
            return unchecked((uint)tail < (uint)VertexCount);
        }

        /// <inheritdoc/>
        public ArraySegment<Endpoints>.Enumerator EnumerateOutEdges(int vertex)
        {
            if (_data is null || _edgesOrderedByTail is null)
                return ArraySegment<Endpoints>.Empty.GetEnumerator();

            ReadOnlySpan<int> upperBoundByVertex = GetUpperBoundByVertex(_data);
            if (unchecked((uint)vertex >= (uint)upperBoundByVertex.Length))
                return ArraySegment<Endpoints>.Empty.GetEnumerator();

            int lowerBound = vertex == 0 ? 0 : upperBoundByVertex[vertex - 1];
            int upperBound = upperBoundByVertex[vertex];
            Debug.Assert(lowerBound <= upperBound, "lowerBound <= upperBound");
            return new ArraySegment<Endpoints>(
                _edgesOrderedByTail, lowerBound, upperBound - lowerBound).GetEnumerator();
        }

        /// <inheritdoc/>
        public bool Equals(SimpleIncidenceGraph other) =>
            _data == other._data && _edgesOrderedByTail == other._edgesOrderedByTail;

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is SimpleIncidenceGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => _data is null || _edgesOrderedByTail is null
            ? 0
            : unchecked(_data.GetHashCode() * 397) ^ _edgesOrderedByTail.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetUpperBoundByVertex(int[] data) => data.AsSpan(2, VertexCount);

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
#endif
