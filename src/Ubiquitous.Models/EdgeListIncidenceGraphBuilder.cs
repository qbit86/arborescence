namespace Ubiquitous.Models
{
    using System;
    using System.Buffers;

    public struct EdgeListIncidenceGraphBuilder : IGraphBuilder<EdgeListIncidenceGraph, int, SourceTargetPair<int>>
    {
        private const int DefaultInitialOutDegree = 4;
        private const int MaxCoreClrArrayLength = 0x7fefffff;

        private int _initialOutDegree;
        private ArrayPrefix<ArrayBuilder<SourceTargetPair<int>>> _outEdges;
        private int _edgeCount;

        public EdgeListIncidenceGraphBuilder(int vertexUpperBound)
        {
            _initialOutDegree = DefaultInitialOutDegree;
            ArrayBuilder<SourceTargetPair<int>>[] outEdges =
                ArrayPool<ArrayBuilder<SourceTargetPair<int>>>.Shared.Rent(vertexUpperBound);
            _outEdges = new ArrayPrefix<ArrayBuilder<SourceTargetPair<int>>>(outEdges, vertexUpperBound);
            _edgeCount = 0;
        }

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
                if (newVertexUpperBound + _edgeCount > MaxCoreClrArrayLength)
                {
                    edge = SourceTargetPair.Create(int.MinValue, int.MinValue);
                    return false;
                }

                ArrayPrefixBuilder.EnsureCapacity(ref _outEdges, newVertexUpperBound);
                Array.Clear(_outEdges.Array, _outEdges.Count, newVertexUpperBound - _outEdges.Count);
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
            var storage = new SourceTargetPair<int>[_edgeCount + VertexUpperBound];
            Span<SourceTargetPair<int>> destReorderedEdges = storage.AsSpan(0, _edgeCount);
            Span<SourceTargetPair<int>> destEdgeBounds = storage.AsSpan(_edgeCount, VertexUpperBound);

            for (int s = 0, currentOffset = 0; s != VertexUpperBound; ++s)
            {
                ReadOnlySpan<SourceTargetPair<int>> currentOutEdges = _outEdges[s].AsSpan();
                Span<SourceTargetPair<int>> destOutEdges =
                    destReorderedEdges.Slice(currentOffset, currentOutEdges.Length);
                currentOutEdges.CopyTo(destOutEdges);
                int lowerBound = currentOffset;
                currentOffset += currentOutEdges.Length;
                destEdgeBounds[s] = SourceTargetPair.Create(lowerBound, currentOutEdges.Length);

                if (_outEdges[s].Buffer != null)
                    ArrayPool<SourceTargetPair<int>>.Shared.Return(_outEdges[s].Buffer, true);
            }

            if (_outEdges.Array != null)
                ArrayPool<ArrayBuilder<SourceTargetPair<int>>>.Shared.Return(_outEdges.Array, true);

            var result = new EdgeListIncidenceGraph(VertexUpperBound, storage);

            _initialOutDegree = DefaultInitialOutDegree;
            _outEdges = ArrayPrefix<ArrayBuilder<SourceTargetPair<int>>>.Empty;
            _edgeCount = 0;

            return result;
        }
    }
}
