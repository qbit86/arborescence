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

        public AdjacencyListIncidenceGraphBuilder(int initialVertexCount) : this(initialVertexCount, 0)
        {
        }

        public AdjacencyListIncidenceGraphBuilder(int initialVertexCount, int edgeCapacity)
        {
            if (initialVertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

            if (edgeCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCapacity));

            _initialOutDegree = DefaultInitialOutDegree;
            int effectiveEdgeCapacity = Math.Max(edgeCapacity, DefaultInitialOutDegree);
            _sources = new ArrayBuilder<int>(effectiveEdgeCapacity);
            _targets = new ArrayBuilder<int>(effectiveEdgeCapacity);
            ArrayBuilder<int>[] outEdges = Pool.Rent(initialVertexCount);
            Array.Clear(outEdges, 0, initialVertexCount);
            _outEdges = new ArrayPrefix<ArrayBuilder<int>>(outEdges, initialVertexCount);
        }

        private static ArrayPool<ArrayBuilder<int>> Pool => ArrayPool<ArrayBuilder<int>>.Shared;

        public int VertexCount => _outEdges.Count;

        public int InitialOutDegree
        {
            get => _initialOutDegree <= 0 ? DefaultInitialOutDegree : _initialOutDegree;
            set => _initialOutDegree = value;
        }

        public void EnsureVertexCount(int vertexCount)
        {
            if (vertexCount > VertexCount)
                _outEdges = ArrayPrefixBuilder.Resize(_outEdges, vertexCount, true);
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
            EnsureVertexCount(max + 1);

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
        // vertexCount    reorderedEdges     sources
        //         ↓↓↓             ↓↓↓↓↓     ↓↓↓↓↓
        //         [4][_^|_^|_^|_^][021][bcb][aca]
        //            ↑↑↑↑↑↑↑↑↑↑↑↑↑     ↑↑↑↑↑
        //               edgeBounds     targets

        public AdjacencyListIncidenceGraph ToGraph()
        {
            Assert(_sources.Count == _targets.Count);
            int vertexCount = VertexCount;
            int sourceCount = _sources.Count;
            int targetCount = _targets.Count;
            var storage = new int[1 + 2 * vertexCount + sourceCount + targetCount + sourceCount];
            storage[0] = vertexCount;

            ReadOnlySpan<ArrayBuilder<int>> outEdges = _outEdges.AsSpan();
            Span<int> destEdgeBounds = storage.AsSpan(1, 2 * vertexCount);
            Span<int> destReorderedEdges = storage.AsSpan(1 + 2 * vertexCount, sourceCount);

            for (int s = 0, currentBound = 0; s != outEdges.Length; ++s)
            {
                ReadOnlySpan<int> currentOutEdges = outEdges[s].AsSpan();
                currentOutEdges.CopyTo(destReorderedEdges.Slice(currentBound, currentOutEdges.Length));
                int finalLeftBound = 1 + 2 * vertexCount + currentBound;
                destEdgeBounds[2 * s] = finalLeftBound;
                destEdgeBounds[2 * s + 1] = currentOutEdges.Length;
                currentBound += currentOutEdges.Length;
                outEdges[s].Dispose(false);
            }

            if (_outEdges.Array != null)
                Pool.Return(_outEdges.Array, true);
            _outEdges = ArrayPrefix<ArrayBuilder<int>>.Empty;

            Span<int> destTargets = storage.AsSpan(1 + 2 * vertexCount + sourceCount, targetCount);
            _targets.AsSpan().CopyTo(destTargets);
            _targets.Dispose(false);

            Span<int> destSources = storage.AsSpan(1 + 2 * vertexCount + sourceCount + targetCount, sourceCount);
            _sources.AsSpan().CopyTo(destSources);
            _sources.Dispose(false);

            return new AdjacencyListIncidenceGraph(storage);
        }
    }
}
