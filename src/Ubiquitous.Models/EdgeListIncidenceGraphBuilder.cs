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

            _rawInitialOutDegree = DefaultInitialOutDegree;
            _outEdges = ArrayPrefix<ArrayBuilder<SourceTargetPair<int>>>.Empty;
            _edgeCount = 0;

            return new EdgeListIncidenceGraph(VertexUpperBound, storage);
        }
    }
}
