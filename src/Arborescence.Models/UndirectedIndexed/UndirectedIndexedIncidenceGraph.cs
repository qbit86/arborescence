#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using static TryHelpers;

    /// <summary>
    /// Represents a forward-traversable graph.
    /// </summary>
    public readonly partial struct UndirectedIndexedIncidenceGraph :
        IHeadIncidence<int, int>,
        ITailIncidence<int, int>,
        IOutEdgesIncidence<int, ArraySegment<int>.Enumerator>,
        IEquatable<UndirectedIndexedIncidenceGraph>
    {
        // Layout:
        // 1    | n — the number of vertices
        // 1    | m — the number of edges
        // n    | upper bounds of out-edge enumerators indexed by vertices
        // 2×m  | edges sorted by tail
        // m    | heads indexed by edges
        // m    | tails indexed by edges
        private readonly int[]? _data;

        internal UndirectedIndexedIncidenceGraph(int[] data)
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
            int edgeIndex = edge < 0 ? ~edge : edge;
            ReadOnlySpan<int> endpointByEdge = edge < 0 ? GetHeadByEdge() : GetTailByEdge();
            return edgeIndex < endpointByEdge.Length ? Some(endpointByEdge[edgeIndex], out tail) : None(out tail);
        }

        /// <inheritdoc/>
        public bool TryGetHead(int edge, out int head)
        {
            int edgeIndex = edge < 0 ? ~edge : edge;
            ReadOnlySpan<int> endpointByEdge = edge < 0 ? GetTailByEdge() : GetHeadByEdge();
            return edgeIndex < endpointByEdge.Length ? Some(endpointByEdge[edgeIndex], out head) : None(out head);
        }

        /// <inheritdoc/>
        public ArraySegment<int>.Enumerator EnumerateOutEdges(int vertex)
        {
            if (_data is null)
                return ArraySegment<int>.Empty.GetEnumerator();

            ReadOnlySpan<int> upperBoundByVertex = GetUpperBoundByVertex(_data);
            if (unchecked((uint)vertex >= (uint)upperBoundByVertex.Length))
                return ArraySegment<int>.Empty.GetEnumerator();

            int lowerBound = vertex == 0 ? 0 : upperBoundByVertex[vertex - 1];
            int upperBound = upperBoundByVertex[vertex];
            Debug.Assert(lowerBound <= upperBound, "lowerBound <= upperBound");
            int offset = 2 + VertexCount;
            return new ArraySegment<int>(_data, offset + lowerBound, upperBound - lowerBound).GetEnumerator();
        }

        /// <inheritdoc/>
        public bool Equals(UndirectedIndexedIncidenceGraph other) => _data == other._data;

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is UndirectedIndexedIncidenceGraph other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => (_data?.GetHashCode()).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetUpperBoundByVertex(int[] data) => data.AsSpan(2, VertexCount);

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
#endif
