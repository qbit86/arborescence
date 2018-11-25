namespace Ubiquitous
{
    using System;
    using System.Buffers;

    public struct EdgeListIncidenceGraphBuilder : IGraphBuilder<EdgeListIncidenceGraph, int, SourceTargetPair<int>>
    {
        private const int DefaultInitialOutDegree = 4;

        private int _rawInitialOutDegree;
        private ArrayPrefix<ArrayBuilder<SourceTargetPair<int>>> _outEdges;
        private int _edgeCount;

        public EdgeListIncidenceGraphBuilder(int vertexUpperBound)
        {
            _rawInitialOutDegree = DefaultInitialOutDegree;
            ArrayBuilder<SourceTargetPair<int>>[] outEdges =
                ArrayPool<ArrayBuilder<SourceTargetPair<int>>>.Shared.Rent(vertexUpperBound);
            _outEdges = new ArrayPrefix<ArrayBuilder<SourceTargetPair<int>>>(outEdges, vertexUpperBound);
            _edgeCount = 0;
        }

        public int VertexUpperBound => _outEdges.Count;

        public int InitialOutDegree
        {
            get => Math.Max(DefaultInitialOutDegree, _rawInitialOutDegree);
            set => _rawInitialOutDegree = value;
        }

        public bool TryAdd(int source, int target, out SourceTargetPair<int> edge)
        {
            ++_edgeCount;

            throw new NotImplementedException();
        }

        // reorderedEdges
        //          ↓↓↓↓↓
        //          [aca][____]
        //          [bcb][^^^^]
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
                int upperBound = currentOffset;
                destEdgeBounds[s] = SourceTargetPair.Create(lowerBound, upperBound);

                ArrayPool<SourceTargetPair<int>>.Shared.Return(_outEdges[s].Buffer, true);
            }

            ArrayPool<ArrayBuilder<SourceTargetPair<int>>>.Shared.Return(_outEdges.Array, true);

            _rawInitialOutDegree = DefaultInitialOutDegree;
            _outEdges = ArrayPrefix<ArrayBuilder<SourceTargetPair<int>>>.Empty;
            _edgeCount = 0;

            return new EdgeListIncidenceGraph(VertexUpperBound, storage);
        }
    }
}
