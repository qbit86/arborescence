namespace Ubiquitous
{
    using System;
    using static System.Diagnostics.Debug;

    public struct SortedAdjacencyListIncidenceGraphBuilder
    {
        private ArrayBuilder<int> _orderedSources;
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

            EdgeUpperBounds = new int[vertexUpperBound];
            _orderedSources = new ArrayBuilder<int>(edgeCount);
            _targets = new ArrayBuilder<int>(edgeCount);
            _lastSource = 0;
        }

        public int VertexUpperBound => EdgeUpperBounds?.Length ?? 0;

        private int[] EdgeUpperBounds { get; set; }

        public bool TryAdd(int source, int target, out int edge)
        {
            if (EdgeUpperBounds == null)
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

            Assert(_orderedSources.Count == _targets.Count);
            int newEdgeIndex = _targets.Count;
            _orderedSources.Add(source);
            _targets.Add(target);

            EdgeUpperBounds[source] = newEdgeIndex + 1;
            _lastSource = source;

            edge = newEdgeIndex;
            return true;
        }

        public SortedAdjacencyListIncidenceGraph Build()
        {
            Assert(_orderedSources.Count == _targets.Count);
            int[] targetsBuffer = _targets.Buffer ?? ArrayBuilder<int>.EmptyArray;
            int[] orderedSourcesBuffer = _orderedSources.Buffer ?? ArrayBuilder<int>.EmptyArray;
            int storageSize = 1 + VertexUpperBound + _targets.Count + _orderedSources.Count;
            var storage = new int[storageSize];

            // Make EdgeUpperBounds monotonic in case if we skipped some sources.
            for (int v = 1; v < EdgeUpperBounds.Length; ++v)
            {
                if (EdgeUpperBounds[v] < EdgeUpperBounds[v - 1])
                    EdgeUpperBounds[v] = EdgeUpperBounds[v - 1];
            }

            storage[0] = VertexUpperBound;
            Array.Copy(EdgeUpperBounds, 0, storage, 1, VertexUpperBound);
            Array.Copy(targetsBuffer, 0, storage, 1 + VertexUpperBound, _targets.Count);
            Array.Copy(orderedSourcesBuffer, 0, storage, 1 + VertexUpperBound + _targets.Count, _orderedSources.Count);

            _orderedSources = default;
            _targets = default;
            EdgeUpperBounds = null;

            return new SortedAdjacencyListIncidenceGraph(storage);
        }
    }
}
