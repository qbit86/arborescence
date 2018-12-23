namespace Ubiquitous.Models
{
    using System;
    using System.Buffers;

    public struct EdgeListIncidenceGraphBuilder : IGraphBuilder<EdgeListIncidenceGraph, int, SourceTargetPair<int>>
    {
        private const int DefaultInitialOutDegree = 4;

        private int _initialOutDegree;
        private ArrayPrefix<ArrayBuilder<SourceTargetPair<int>>> _outEdges;
        private int _edgeCount;

        public EdgeListIncidenceGraphBuilder(int initialVertexUpperBound)
        {
            if (initialVertexUpperBound < 0)
                throw new ArgumentOutOfRangeException(nameof(initialVertexUpperBound));

            _initialOutDegree = DefaultInitialOutDegree;
            ArrayBuilder<SourceTargetPair<int>>[] outEdges = Pool.Rent(initialVertexUpperBound);
            Array.Clear(outEdges, 0, initialVertexUpperBound);
            _outEdges = new ArrayPrefix<ArrayBuilder<SourceTargetPair<int>>>(outEdges, initialVertexUpperBound);
            _edgeCount = 0;
        }

        private static ArrayPool<ArrayBuilder<SourceTargetPair<int>>> Pool =>
            ArrayPool<ArrayBuilder<SourceTargetPair<int>>>.Shared;

        public int VertexUpperBound => _outEdges.Count;

        public int InitialOutDegree
        {
            get => _initialOutDegree <= 0 ? DefaultInitialOutDegree : _initialOutDegree;
            set => _initialOutDegree = value;
        }

        public bool TryAdd(int source, int target, out SourceTargetPair<int> edge)
        {
            if (source < 0)
            {
                edge = SourceTargetPair.Create(-1, -1);
                return false;
            }

            if (target < 0)
            {
                edge = SourceTargetPair.Create(-2, -2);
                return false;
            }

            int max = Math.Max(source, target);
            if (max >= VertexUpperBound)
            {
                int newVertexUpperBound = max + 1;
                int oldCount = _outEdges.Count;
                ArrayPrefixBuilder.Resize(ref _outEdges, newVertexUpperBound, true);
                Array.Clear(_outEdges.Array, oldCount, newVertexUpperBound - oldCount);
            }

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
            int vertexUpperBound = VertexUpperBound;
            var storage = new SourceTargetPair<int>[_edgeCount + vertexUpperBound];
            Span<SourceTargetPair<int>> destReorderedEdges = storage.AsSpan(0, _edgeCount);
            Span<SourceTargetPair<int>> destEdgeBounds = storage.AsSpan(_edgeCount, vertexUpperBound);

            for (int s = 0, currentOffset = 0; s != vertexUpperBound; ++s)
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

            return new EdgeListIncidenceGraph(vertexUpperBound, storage);
        }
    }
}
