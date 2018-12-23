namespace Ubiquitous.Models
{
    using System;
    using System.Buffers;
    using static System.Diagnostics.Debug;

    public struct AdjacencyListIncidenceGraphBuilder : IGraphBuilder<AdjacencyListIncidenceGraph, int, int>
    {
        private const int DefaultInitialOutDegree = 4;

        private int _initialOutDegree;
        private ArrayBuilder<int> _sources;
        private ArrayBuilder<int> _targets;
        private ArrayPrefix<ArrayBuilder<int>> _outEdges;

        public AdjacencyListIncidenceGraphBuilder(int initialVertexUpperBound) : this(initialVertexUpperBound, 0)
        {
        }

        public AdjacencyListIncidenceGraphBuilder(int initialVertexUpperBound, int edgeCount)
        {
            if (initialVertexUpperBound < 0)
                throw new ArgumentOutOfRangeException(nameof(initialVertexUpperBound));

            if (edgeCount < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCount));

            _initialOutDegree = DefaultInitialOutDegree;
            int initialEdgeCount = Math.Max(edgeCount, DefaultInitialOutDegree);
            _sources = new ArrayBuilder<int>(initialEdgeCount);
            _targets = new ArrayBuilder<int>(initialEdgeCount);
            ArrayBuilder<int>[] outEdges = Pool.Rent(initialVertexUpperBound);
            Array.Clear(outEdges, 0, initialVertexUpperBound);
            _outEdges = new ArrayPrefix<ArrayBuilder<int>>(outEdges, initialVertexUpperBound);
        }

        private static ArrayPool<ArrayBuilder<int>> Pool => ArrayPool<ArrayBuilder<int>>.Shared;

        public int VertexUpperBound => _outEdges.Count;

        public int InitialOutDegree
        {
            get => _initialOutDegree <= 0 ? DefaultInitialOutDegree : _initialOutDegree;
            set => _initialOutDegree = value;
        }

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
            if (max >= VertexUpperBound)
            {
                int newVertexUpperBound = max + 1;
                int oldCount = _outEdges.Count;
                ArrayPrefixBuilder.EnsureCapacity(ref _outEdges, newVertexUpperBound, true);
                Array.Clear(_outEdges.Array, oldCount, newVertexUpperBound - oldCount);
            }

            Assert(_sources.Count == _targets.Count);
            int newEdgeIndex = _targets.Count;
            _sources.Add(source);
            _targets.Add(target);

            if (_outEdges[source].Buffer == null)
                _outEdges[source] = new ArrayBuilder<int>(InitialOutDegree);

            _outEdges.Array[source].Add(newEdgeIndex);

            edge = newEdgeIndex;
            return true;
        }

        // Storage layout:
        // vertexUpperBound    reorderedEdges     sources
        //              ↓↓↓             ↓↓↓↓↓     ↓↓↓↓↓
        //              [4][_^|_^|_^|_^][021][bcb][aca]
        //                 ↑↑↑↑↑↑↑↑↑↑↑↑↑     ↑↑↑↑↑
        //                    edgeBounds     targets

        public AdjacencyListIncidenceGraph ToGraph()
        {
            Assert(_sources.Count == _targets.Count);
            int vertexUpperBound = VertexUpperBound;
            var storage = new int[1 + 2 * vertexUpperBound + _sources.Count + _targets.Count + _sources.Count];
            storage[0] = vertexUpperBound;

            ReadOnlySpan<ArrayBuilder<int>> outEdges = _outEdges.AsSpan();
            Span<int> destEdgeBounds = storage.AsSpan(1, 2 * vertexUpperBound);
            Span<int> destReorderedEdges = storage.AsSpan(1 + 2 * vertexUpperBound, _sources.Count);

            for (int s = 0, currentBound = 0; s != outEdges.Length; ++s)
            {
                ReadOnlySpan<int> currentOutEdges = outEdges[s].AsSpan();
                currentOutEdges.CopyTo(destReorderedEdges.Slice(currentBound, currentOutEdges.Length));
                int finalLeftBound = 1 + 2 * vertexUpperBound + currentBound;
                destEdgeBounds[2 * s] = finalLeftBound;
                destEdgeBounds[2 * s + 1] = currentOutEdges.Length;
                currentBound += currentOutEdges.Length;
                outEdges[s].Dispose(false);
            }

            if (_outEdges.Array != null)
                Pool.Return(_outEdges.Array, true);
            _outEdges = ArrayPrefix<ArrayBuilder<int>>.Empty;

            Span<int> destTargets = storage.AsSpan(1 + 2 * vertexUpperBound + _sources.Count, _targets.Count);
            _targets.AsSpan().CopyTo(destTargets);
            _targets.Dispose(false);

            Span<int> destSources = storage.AsSpan(1 + 2 * vertexUpperBound + _sources.Count + _targets.Count,
                _sources.Count);
            _sources.AsSpan().CopyTo(destSources);
            _sources.Dispose(false);

            return new AdjacencyListIncidenceGraph(storage);
        }
    }
}
