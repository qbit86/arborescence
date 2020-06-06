namespace Ubiquitous.Models
{
    using System;
    using System.Buffers;
    using static System.Diagnostics.Debug;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct AdjacencyListIncidenceGraphBuilder : IGraphBuilder<AdjacencyListIncidenceGraph, int, int>
    {
        private const int DefaultInitialOutDegree = 4;

        private int _initialOutDegree;
        private ArrayBuilder<int> _tails;
        private ArrayBuilder<int> _heads;
        private ArrayPrefix<ArrayBuilder<int>> _outEdges;

        public AdjacencyListIncidenceGraphBuilder(int initialVertexCount, int edgeCapacity = 0)
        {
            if (initialVertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

            if (edgeCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCapacity));

            _initialOutDegree = DefaultInitialOutDegree;
            int effectiveEdgeCapacity = Math.Max(edgeCapacity, DefaultInitialOutDegree);
            _tails = new ArrayBuilder<int>(effectiveEdgeCapacity);
            _heads = new ArrayBuilder<int>(effectiveEdgeCapacity);
            ArrayBuilder<int>[] outEdges = Pool.Rent(initialVertexCount);
            Array.Clear(outEdges, 0, initialVertexCount);
            _outEdges = ArrayPrefix.Create(outEdges, initialVertexCount);
        }

        private static ArrayPool<ArrayBuilder<int>> Pool => ArrayPool<ArrayBuilder<int>>.Shared;

        public int VertexCount => _outEdges.Count;

        public int InitialOutDegree
        {
            get => _initialOutDegree <= 0 ? DefaultInitialOutDegree : _initialOutDegree;
            set => _initialOutDegree = value;
        }

        public void EnsureVertexCount(int vertexCount)
        {
            if (vertexCount > VertexCount)
                _outEdges = ArrayPrefixBuilder.Resize(_outEdges, vertexCount, true);
        }

        public bool TryAdd(int tail, int head, out int edge)
        {
            if (tail < 0)
            {
                edge = default;
                return false;
            }

            if (head < 0)
            {
                edge = default;
                return false;
            }

            int max = Math.Max(tail, head);
            EnsureVertexCount(max + 1);

            Assert(_tails.Count == _heads.Count);
            int newEdgeIndex = _heads.Count;
            _tails.Add(tail);
            _heads.Add(head);

            if (_outEdges[tail].Buffer == null)
                _outEdges[tail] = new ArrayBuilder<int>(InitialOutDegree);

            _outEdges.Array[tail].Add(newEdgeIndex);

            edge = newEdgeIndex;
            return true;
        }

        // Storage layout:
        // vertexCount    reorderedEdges     tails
        //         ↓↓↓             ↓↓↓↓↓     ↓↓↓↓↓
        //         [4][_^|_^|_^|_^][021][bcb][aca]
        //            ↑↑↑↑↑↑↑↑↑↑↑↑↑     ↑↑↑↑↑
        //               edgeBounds     heads

        public AdjacencyListIncidenceGraph ToGraph()
        {
            Assert(_tails.Count == _heads.Count);
            int vertexCount = VertexCount;
            int tailCount = _tails.Count;
            int headCount = _heads.Count;
            var storage = new int[1 + 2 * vertexCount + tailCount + headCount + tailCount];
            storage[0] = vertexCount;

            Span<ArrayBuilder<int>> outEdges = _outEdges.AsSpan();
            Span<int> destEdgeBounds = storage.AsSpan(1, 2 * vertexCount);
            Span<int> destReorderedEdges = storage.AsSpan(1 + 2 * vertexCount, tailCount);

            for (int s = 0, currentBound = 0; s != outEdges.Length; ++s)
            {
                ReadOnlySpan<int> currentOutEdges = outEdges[s].AsSpan();
                currentOutEdges.CopyTo(destReorderedEdges.Slice(currentBound, currentOutEdges.Length));
                int finalLeftBound = 1 + 2 * vertexCount + currentBound;
                destEdgeBounds[2 * s] = finalLeftBound;
                destEdgeBounds[2 * s + 1] = currentOutEdges.Length;
                currentBound += currentOutEdges.Length;
                outEdges[s].Dispose(false);
            }

            if (_outEdges.Array != null)
                Pool.Return(_outEdges.Array, true);
            _outEdges = ArrayPrefix<ArrayBuilder<int>>.Empty;

            Span<int> destHeads = storage.AsSpan(1 + 2 * vertexCount + tailCount, headCount);
            _heads.AsSpan().CopyTo(destHeads);
            _heads.Dispose(false);

            Span<int> destTails = storage.AsSpan(1 + 2 * vertexCount + tailCount + headCount, tailCount);
            _tails.AsSpan().CopyTo(destTails);
            _tails.Dispose(false);

            return new AdjacencyListIncidenceGraph(storage);
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
