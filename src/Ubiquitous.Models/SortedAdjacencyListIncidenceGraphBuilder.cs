namespace Ubiquitous
{
    using System;
    using static System.Diagnostics.Debug;

    public struct SortedAdjacencyListIncidenceGraphBuilder
    {
        private ArrayBuilder<int> _sources;
        private ArrayBuilder<int> _targets;
        private int _lastSource;

        public SortedAdjacencyListIncidenceGraphBuilder(int vertexUpperBound) : this(vertexUpperBound, 0)
        {
        }

        public SortedAdjacencyListIncidenceGraphBuilder(int vertexUpperBound, int edgeCount)
        {
            if (vertexUpperBound < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexUpperBound));

            if (edgeCount < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCount));

            EdgeBounds = new int[vertexUpperBound];
            _sources = new ArrayBuilder<int>(edgeCount);
            _targets = new ArrayBuilder<int>(edgeCount);
            _lastSource = 0;
        }

        public int VertexUpperBound => EdgeBounds?.Length ?? 0;

        private int[] EdgeBounds { get; set; }

        public int Add(int source, int target)
        {
            if (EdgeBounds == null)
                return int.MinValue;

            if ((uint)source >= (uint)VertexUpperBound)
                return -1;

            if ((uint)target >= (uint)VertexUpperBound)
                return -2;

            if (source < _lastSource)
            {
                return sbyte.MinValue;
            }

            Assert(_sources.Count == _targets.Count);
            int newEdgeIndex = _targets.Count;
            _sources.Add(source);
            _targets.Add(target);

            EdgeBounds[source] = newEdgeIndex + 1;
            _lastSource = source;

            return newEdgeIndex;
        }

        public SortedAdjacencyListIncidenceGraph Build()
        {
            Assert(_sources.Count == _targets.Count);
            int[] targetsBuffer = _targets.Buffer ?? ArrayBuilder<int>.EmptyArray;
            int[] sourcesBuffer = _sources.Buffer ?? ArrayBuilder<int>.EmptyArray;
            int storageSize = 1 + VertexUpperBound + _targets.Count + _sources.Count;
            int[] storage = new int[storageSize];

            // Make EdgeBounds monotonic in case if we skipped some sources.
            for (int v = 1; v < EdgeBounds.Length; ++v)
            {
                if (EdgeBounds[v] < EdgeBounds[v - 1])
                    EdgeBounds[v] = EdgeBounds[v - 1];
            }

            _sources = default;
            _targets = default;
            EdgeBounds = null;

            return new SortedAdjacencyListIncidenceGraph(storage);
        }
    }
}
