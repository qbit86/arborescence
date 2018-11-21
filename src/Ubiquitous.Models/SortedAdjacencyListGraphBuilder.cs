namespace Ubiquitous
{
    using System;

    public struct SortedAdjacencyListGraphBuilder
    {
        private ArrayBuilder<int> _sources;
        private ArrayBuilder<int> _targets;
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

            _sources = new ArrayBuilder<int>(edgeCount);
            _targets = new ArrayBuilder<int>(edgeCount);
            _needsSorting = false;
            VertexCount = vertexCount;
        }

        public int VertexCount { get; }

        public int Add(SourceTargetPair<int> edge)
        {
            if ((uint)edge.Source >= (uint)VertexCount)
                return -1;

            if ((uint)edge.Target >= (uint)VertexCount)
                return -1;

            throw new NotImplementedException();
        }
    }
}
