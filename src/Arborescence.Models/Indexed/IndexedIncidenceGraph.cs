namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using static TryHelpers;
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
    using EdgeEnumerator = System.ArraySegment<int>.Enumerator;
#else
    using System.Collections.Generic;
    using System.Linq;
    using EdgeEnumerator = System.Collections.Generic.IEnumerator<int>;
#endif

    /// <summary>
    /// Represents a forward-traversable graph.
    /// </summary>
    public readonly partial struct IndexedIncidenceGraph :
        IHeadIncidence<int, int>,
        ITailIncidence<int, int>,
        IOutEdgesIncidence<int, EdgeEnumerator>,
        IEquatable<IndexedIncidenceGraph>
    {
        // Layout:
        // 1    | n — the number of vertices
        // 1    | m — the number of edges
        // n    | upper bounds of out-edge enumerators indexed by vertices
        // m    | edges sorted by tail
        // m    | heads indexed by edges
        // m    | tails indexed by edges
        private readonly int[]? _data;

        internal IndexedIncidenceGraph(int[] data)
        {
            Debug.Assert(data.Length >= 2, "data.Length >= 2");
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
            ReadOnlySpan<int> tailByEdge = GetTailByEdge();
            return unchecked((uint)edge < (uint)tailByEdge.Length) ? Some(tailByEdge[edge], out tail) : None(out tail);
        }

        /// <inheritdoc/>
        public bool TryGetHead(int edge, out int head)
        {
            ReadOnlySpan<int> headByEdge = GetHeadByEdge();
            return unchecked((uint)edge < (uint)headByEdge.Length) ? Some(headByEdge[edge], out head) : None(out head);
        }

        /// <inheritdoc/>
        public EdgeEnumerator EnumerateOutEdges(int vertex)
        {
            if (_data is null)
                return EmptyEnumerator();

            ReadOnlySpan<int> upperBoundByVertex = GetUpperBoundByVertex(_data);
            if (unchecked((uint)vertex >= (uint)upperBoundByVertex.Length))
                return EmptyEnumerator();

            int lowerBound = vertex == 0 ? 0 : upperBoundByVertex[vertex - 1];
            int upperBound = upperBoundByVertex[vertex];
            Debug.Assert(lowerBound <= upperBound, "lowerBound <= upperBound");
            int offset = 2 + VertexCount;
            return GetEnumerator(new(_data, offset + lowerBound, upperBound - lowerBound));
        }

        /// <inheritdoc/>
        public bool Equals(IndexedIncidenceGraph other) => _data == other._data;

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is IndexedIncidenceGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => (_data?.GetHashCode()).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetUpperBoundByVertex(int[] data) => data.AsSpan(2, VertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTailByEdge() =>
            IsDefault ? ReadOnlySpan<int>.Empty : _data.AsSpan(2 + VertexCount + EdgeCount + EdgeCount, EdgeCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetHeadByEdge() =>
            IsDefault ? ReadOnlySpan<int>.Empty : _data.AsSpan(2 + VertexCount + EdgeCount, EdgeCount);

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

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static EdgeEnumerator EmptyEnumerator() => ArraySegment<int>.Empty.GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static EdgeEnumerator GetEnumerator(ArraySegment<int> arraySegment) => arraySegment.GetEnumerator();
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static EdgeEnumerator EmptyEnumerator() => Enumerable.Empty<int>().GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static EdgeEnumerator GetEnumerator(ArraySegment<int> arraySegment) =>
            ((IEnumerable<int>)arraySegment).GetEnumerator();
#endif
    }
}
