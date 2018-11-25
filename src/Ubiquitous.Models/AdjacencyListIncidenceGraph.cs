namespace Ubiquitous
{
    using System;
    using System.Runtime.CompilerServices;
    using static System.Diagnostics.Debug;

    public readonly struct AdjacencyListIncidenceGraph : IIncidenceGraph<int, int, ArraySegmentEnumerator<int>>,
        IEquatable<AdjacencyListIncidenceGraph>
    {
        private readonly int[] _storage;

        internal AdjacencyListIncidenceGraph(int[] storage)
        {
            Assert(storage != null, "storage != null");
            Assert(storage.Length > 0, "storage.Length > 0");

            Assert(storage[0] >= 0, "storage[0] >= 0");
            Assert(storage[0] <= storage.Length - 1, "storage[0] <= storage.Length - 1");

            _storage = storage;
        }

        public int VertexUpperBound => _storage == null ? 0 : GetVertexUpperBound();

        public int EdgeCount => _storage == null ? 0 : (_storage.Length - 1 - 2 * GetVertexUpperBound()) / 3;

        public bool TryGetSource(int edge, out int source)
        {
            ReadOnlySpan<int> sources = GetSources();
            if ((uint)edge >= (uint)sources.Length)
            {
                source = -1;
                return false;
            }

            source = sources[edge];
            return true;
        }

        public bool TryGetTarget(int edge, out int target)
        {
            ReadOnlySpan<int> targets = GetTargets();
            if ((uint)edge >= (uint)targets.Length)
            {
                target = -1;
                return false;
            }

            target = targets[edge];
            return true;
        }

        public bool TryGetOutEdges(int vertex, out ArraySegmentEnumerator<int> outEdges)
        {
            ReadOnlySpan<int> edgeBounds = GetEdgeBounds();

            if ((uint)(2 * vertex) >= (uint)edgeBounds.Length)
            {
                outEdges = new ArraySegmentEnumerator<int>(ArrayBuilder<int>.EmptyArray, 0, 0);
                return false;
            }

            int start = edgeBounds[2 * vertex];
            int endExclusive = edgeBounds[2 * vertex + 1];
            if (endExclusive < start)
            {
                outEdges = new ArraySegmentEnumerator<int>(ArrayBuilder<int>.EmptyArray, 0, 0);
                return false;
            }

            outEdges = new ArraySegmentEnumerator<int>(_storage, start, endExclusive);
            return true;
        }

        public bool Equals(AdjacencyListIncidenceGraph other)
        {
            return _storage == other._storage;
        }

        public override bool Equals(object obj)
        {
            return obj is AdjacencyListIncidenceGraph other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _storage?.GetHashCode() ?? 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetEdgeBounds()
        {
            return _storage.AsSpan().Slice(1, 2 * VertexUpperBound);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetSources()
        {
            return _storage.AsSpan().Slice(1 + 2 * VertexUpperBound + 2 * EdgeCount, EdgeCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<int> GetTargets()
        {
            return _storage.AsSpan().Slice(1 + 2 * VertexUpperBound + EdgeCount, EdgeCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetVertexUpperBound()
        {
            Assert(_storage != null, "_storage != null");
            Assert(_storage.Length > 0, "_storage.Length > 0");

            int result = _storage[0];
            Assert(result >= 0, "result >= 0");
            Assert(2 * result <= _storage.Length - 1, "2 * result <= _storage.Length - 1");

            return result;
        }
    }
}
