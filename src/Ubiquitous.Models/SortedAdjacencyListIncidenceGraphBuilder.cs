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

        public bool TryAdd(int source, int target, out int edge)
        {
            if (EdgeBounds == null)
            {
                edge = int.MinValue;
                return false;
            }

            if ((uint)source >= (uint)VertexUpperBound)
            {
                edge = -1;
                return false;
            }

            if ((uint)target >= (uint)VertexUpperBound)
            {
                edge = -2;
                return false;
            }

            if (source < _lastSource)
            {
                edge = sbyte.MinValue;
                return false;
            }

            Assert(_sources.Count == _targets.Count);
            int newEdgeIndex = _targets.Count;
            _sources.Add(source);
            _targets.Add(target);

            EdgeBounds[source] = newEdgeIndex + 1;
            _lastSource = source;

            edge = newEdgeIndex;
            return true;
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
