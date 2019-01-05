namespace Ubiquitous.Models
{
    using System;
    using System.Buffers;
    using static System.Diagnostics.Debug;

    public struct SortedAdjacencyListIncidenceGraphBuilder : IGraphBuilder<SortedAdjacencyListIncidenceGraph, int, int>
    {
        private ArrayBuilder<int> _orderedSources;
        private ArrayBuilder<int> _targets;
        private ArrayPrefix<int> _edgeUpperBounds;
        private int _lastSource;

        public SortedAdjacencyListIncidenceGraphBuilder(int initialVertexCount) : this(initialVertexCount, 0)
        {
        }

        public SortedAdjacencyListIncidenceGraphBuilder(int initialVertexCount, int edgeCapacity)
        {
            if (initialVertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

            if (edgeCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCapacity));

            _orderedSources = new ArrayBuilder<int>(edgeCapacity);
            _targets = new ArrayBuilder<int>(edgeCapacity);
            _lastSource = 0;
            int[] edgeUpperBounds = Pool.Rent(initialVertexCount);
            Array.Clear(edgeUpperBounds, 0, initialVertexCount);
            _edgeUpperBounds = new ArrayPrefix<int>(edgeUpperBounds, initialVertexCount);
        }

        private static ArrayPool<int> Pool => ArrayPool<int>.Shared;

        public int VertexCount => _edgeUpperBounds.Count;

        public bool TryAdd(int source, int target, out int edge)
        {
            if (source < 0)
            {
                edge = -1;
                return false;
            }

            if (target < 0)
            {
                edge = -2;
                return false;
            }

            int max = Math.Max(source, target);
            if (max >= VertexCount)
            {
                int newVertexCount = max + 1;
                _edgeUpperBounds = ArrayPrefixBuilder.Resize(_edgeUpperBounds, newVertexCount, true);
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

            _edgeUpperBounds[source] = newEdgeIndex + 1;
            _lastSource = source;

            edge = newEdgeIndex;
            return true;
        }

        // Storage layout:
        // vertexCount      targets
        //         ↓↓↓      ↓↓↓↓↓
        //         [4][^^^^][bbc][aac]
        //            ↑↑↑↑↑↑     ↑↑↑↑↑
        //   edgeUpperBounds     orderedSources

        public SortedAdjacencyListIncidenceGraph ToGraph()
        {
            Assert(_orderedSources.Count == _targets.Count);
            int vertexCount = VertexCount;
            int targetCount = _targets.Count;
            int orderedSourceCount = _orderedSources.Count;
            var storage = new int[1 + vertexCount + targetCount + orderedSourceCount];
            storage[0] = vertexCount;

            ReadOnlySpan<int> targetsBuffer = _targets.AsSpan();
            targetsBuffer.CopyTo(storage.AsSpan(1 + vertexCount, targetCount));
            _targets.Dispose(false);

            ReadOnlySpan<int> orderedSourcesBuffer = _orderedSources.AsSpan();
            orderedSourcesBuffer.CopyTo(storage.AsSpan(1 + vertexCount + targetCount, orderedSourceCount));
            _orderedSources.Dispose(false);

            // Make EdgeUpperBounds monotonic in case if we skipped some sources.
            for (int v = 1; v < _edgeUpperBounds.Count; ++v)
            {
                if (_edgeUpperBounds[v] < _edgeUpperBounds[v - 1])
                    _edgeUpperBounds[v] = _edgeUpperBounds[v - 1];
            }

            _edgeUpperBounds.CopyTo(storage, 1);
            if (_edgeUpperBounds.Array != null)
                Pool.Return(_edgeUpperBounds.Array);
            _edgeUpperBounds = ArrayPrefix<int>.Empty;

            _lastSource = 0;

            return new SortedAdjacencyListIncidenceGraph(storage);
        }
    }
}
