namespace Ubiquitous.Models
{
    using System;
    using System.Buffers;

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct EdgeListIncidenceGraphBuilder : IGraphBuilder<EdgeListIncidenceGraph, int, SourceTargetPair<int>>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        private const int DefaultInitialOutDegree = 4;

        private int _initialOutDegree;
        private ArrayPrefix<ArrayBuilder<SourceTargetPair<int>>> _outEdges;
        private int _edgeCount;

        public EdgeListIncidenceGraphBuilder(int initialVertexCount)
        {
            if (initialVertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

            _initialOutDegree = DefaultInitialOutDegree;
            ArrayBuilder<SourceTargetPair<int>>[] outEdges = Pool.Rent(initialVertexCount);
            Array.Clear(outEdges, 0, initialVertexCount);
            _outEdges = ArrayPrefix.Create(outEdges, initialVertexCount);
            _edgeCount = 0;
        }

        private static ArrayPool<ArrayBuilder<SourceTargetPair<int>>> Pool =>
            ArrayPool<ArrayBuilder<SourceTargetPair<int>>>.Shared;

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

        public bool TryAdd(int source, int target, out SourceTargetPair<int> edge)
        {
            if (source < 0)
            {
                edge = default;
                return false;
            }

            if (target < 0)
            {
                edge = default;
                return false;
            }

            int max = Math.Max(source, target);
            EnsureVertexCount(max + 1);

            if (_outEdges[source].Buffer == null)
                _outEdges[source] = new ArrayBuilder<SourceTargetPair<int>>(InitialOutDegree);

            edge = SourceTargetPair.Create(source, target);
            _outEdges.Array[source].Add(edge);
            ++_edgeCount;

            return true;
        }

        // reorderedEdges
        //          ↓↓↓↓↓
        //          [aac][____]
        //          [bbc][^^^^]
        //               ↑↑↑↑↑↑
        //               edgeBounds

        public EdgeListIncidenceGraph ToGraph()
        {
            int vertexCount = VertexCount;
            var storage = new SourceTargetPair<int>[_edgeCount + vertexCount];
            Span<SourceTargetPair<int>> destReorderedEdges = storage.AsSpan(0, _edgeCount);
            Span<SourceTargetPair<int>> destEdgeBounds = storage.AsSpan(_edgeCount, vertexCount);

            for (int s = 0, currentOffset = 0; s != vertexCount; ++s)
            {
                ReadOnlySpan<SourceTargetPair<int>> currentOutEdges = _outEdges[s].AsSpan();
                Span<SourceTargetPair<int>> destOutEdges =
                    destReorderedEdges.Slice(currentOffset, currentOutEdges.Length);
                currentOutEdges.CopyTo(destOutEdges);
                int lowerBound = currentOffset;
                currentOffset += currentOutEdges.Length;
                destEdgeBounds[s] = SourceTargetPair.Create(lowerBound, currentOutEdges.Length);

                _outEdges[s].Dispose(false);
            }

            if (_outEdges.Array != null)
                Pool.Return(_outEdges.Array, true);
            _outEdges = ArrayPrefix<ArrayBuilder<SourceTargetPair<int>>>.Empty;
            _edgeCount = 0;

            return new EdgeListIncidenceGraph(vertexCount, storage);
        }
    }
}
