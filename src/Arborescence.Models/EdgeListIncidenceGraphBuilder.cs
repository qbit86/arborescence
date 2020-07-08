namespace Arborescence.Models
{
    using System;
    using System.Buffers;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct EdgeListIncidenceGraphBuilder : IGraphBuilder<EdgeListIncidenceGraph, int, Endpoints<int>>
    {
        private const int DefaultInitialOutDegree = 4;

        private int _initialOutDegree;
        private ArrayPrefix<ArrayBuilder<Endpoints<int>>> _outEdges;
        private int _edgeCount;

        public EdgeListIncidenceGraphBuilder(int initialVertexCount)
        {
            if (initialVertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

            _initialOutDegree = DefaultInitialOutDegree;
            ArrayBuilder<Endpoints<int>>[] outEdges = Pool.Rent(initialVertexCount);
            Array.Clear(outEdges, 0, initialVertexCount);
            _outEdges = ArrayPrefix.Create(outEdges, initialVertexCount);
            _edgeCount = 0;
        }

        private static ArrayPool<ArrayBuilder<Endpoints<int>>> Pool =>
            ArrayPool<ArrayBuilder<Endpoints<int>>>.Shared;

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

        /// <inheritdoc/>
        public bool TryAdd(int tail, int head, out Endpoints<int> edge)
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

            if (_outEdges[tail].Buffer == null)
                _outEdges[tail] = new ArrayBuilder<Endpoints<int>>(InitialOutDegree);

            edge = Endpoints.Create(tail, head);
            _outEdges.Array[tail].Add(edge);
            ++_edgeCount;

            return true;
        }

        // reorderedEdges
        //          ↓↓↓↓↓
        //          [aac][____]
        //          [bbc][^^^^]
        //               ↑↑↑↑↑↑
        //               edgeBounds

        /// <inheritdoc/>
        public EdgeListIncidenceGraph ToGraph()
        {
            int vertexCount = VertexCount;
            var storage = new Endpoints<int>[_edgeCount + vertexCount];
            Span<Endpoints<int>> destReorderedEdges = storage.AsSpan(0, _edgeCount);
            Span<Endpoints<int>> destEdgeBounds = storage.AsSpan(_edgeCount, vertexCount);

            for (int s = 0, currentOffset = 0; s < vertexCount; ++s)
            {
                ReadOnlySpan<Endpoints<int>> currentOutEdges = _outEdges[s].AsSpan();
                Span<Endpoints<int>> destOutEdges =
                    destReorderedEdges.Slice(currentOffset, currentOutEdges.Length);
                currentOutEdges.CopyTo(destOutEdges);
                int lowerBound = currentOffset;
                currentOffset += currentOutEdges.Length;
                destEdgeBounds[s] = Endpoints.Create(lowerBound, currentOutEdges.Length);

                _outEdges[s].Dispose(false);
            }

            if (_outEdges.Array != null)
                Pool.Return(_outEdges.Array, true);
            _outEdges = ArrayPrefix<ArrayBuilder<Endpoints<int>>>.Empty;
            _edgeCount = 0;

            return new EdgeListIncidenceGraph(vertexCount, storage);
        }
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
