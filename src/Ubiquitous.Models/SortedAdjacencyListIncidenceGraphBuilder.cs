namespace Ubiquitous
{
    using System;
    using static System.Diagnostics.Debug;

    public struct SortedAdjacencyListIncidenceGraphBuilder
    {
        private ArrayBuilder<int> _sources;
        private ArrayBuilder<int> _targets;

        public SortedAdjacencyListIncidenceGraphBuilder(int vertexUpperBound) : this(vertexUpperBound, 0)
        {
        }

        public SortedAdjacencyListIncidenceGraphBuilder(int vertexUpperBound, int edgeCount)
        {
            if (vertexUpperBound < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexUpperBound));

            if (edgeCount < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCount));

            _sources = new ArrayBuilder<int>(edgeCount);
            _targets = new ArrayBuilder<int>(edgeCount);
            VertexUpperBound = vertexUpperBound;
        }

        public int VertexUpperBound { get; }

        public int Add(int source, int target)
        {
            if ((uint)source >= (uint)VertexUpperBound)
                return -1;

            if ((uint)target >= (uint)VertexUpperBound)
                return -1;

            Assert(_sources.Count == _targets.Count);
            int newEdgeIndex = _targets.Count;
            _sources.Add(source);
            _targets.Add(target);

            return newEdgeIndex;
        }
    }
}
