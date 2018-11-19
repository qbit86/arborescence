namespace Ubiquitous
{
    using System;

    public struct SortedAdjacencyListGraphBuilder
    {
        private ArrayBuilder<SourceTargetPair<int>> _endpoints;
        private bool _needsSorting;

        public SortedAdjacencyListGraphBuilder(int vertexCount) : this(vertexCount, 0)
        {
        }

        public SortedAdjacencyListGraphBuilder(int vertexCount, int edgeCount)
        {
            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if (edgeCount < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCount));

            _endpoints = new ArrayBuilder<SourceTargetPair<int>>(edgeCount);
            _needsSorting = false;
            VertexCount = vertexCount;
        }

        public int VertexCount { get; }

        public int Add(SourceTargetPair<int> edge)
        {
            // TODO:
            if (_endpoints.Buffer == null)
                throw new InvalidOperationException();

            if ((uint)edge.Source >= (uint)VertexCount)
                return -1;

            if ((uint)edge.Target >= (uint)VertexCount)
                return -1;

            throw new NotImplementedException();
        }
    }
}
