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

        public AdjacencyListIncidenceGraphBuilder(int vertexUpperBound) : this(vertexUpperBound, 0)
        {
        }

        public AdjacencyListIncidenceGraphBuilder(int vertexUpperBound, int edgeCount)
        {
            if (vertexUpperBound < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexUpperBound));

            if (edgeCount < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCount));

            _initialOutDegree = DefaultInitialOutDegree;
            _sources = new ArrayBuilder<int>(edgeCount);
            _targets = new ArrayBuilder<int>(edgeCount);
            VertexUpperBound = vertexUpperBound;
            OutEdges = new ArrayBuilder<int>[vertexUpperBound];
        }

        public int VertexUpperBound { get; set; }

        public int InitialOutDegree
        {
            get => _initialOutDegree <= 0 ? DefaultInitialOutDegree : _initialOutDegree;
            set => _initialOutDegree = value;
        }

        private ArrayBuilder<int>[] OutEdges { get; set; }

        public bool TryAdd(int source, int target, out int edge)
        {
            if (OutEdges == null)
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

            Assert(_sources.Count == _targets.Count);
            int newEdgeIndex = _targets.Count;
            _sources.Add(source);
            _targets.Add(target);

            if (OutEdges[source].Buffer == null)
                OutEdges[source] = new ArrayBuilder<int>(InitialOutDegree);

            OutEdges[source].Add(newEdgeIndex);

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
            var storage = new int[1 + 2 * VertexUpperBound + _sources.Count + _targets.Count + _sources.Count];
            storage[0] = VertexUpperBound;

            ReadOnlySpan<ArrayBuilder<int>> outEdges = OutEdges.AsSpan();
            Span<int> destEdgeBounds = storage.AsSpan(1, 2 * VertexUpperBound);
            Span<int> destReorderedEdges = storage.AsSpan(1 + 2 * VertexUpperBound, _sources.Count);

            for (int s = 0, currentBound = 0; s != outEdges.Length; ++s)
            {
                ReadOnlySpan<int> currentOutEdges = outEdges[s].AsSpan();
                currentOutEdges.CopyTo(destReorderedEdges.Slice(currentBound, currentOutEdges.Length));
                int finalLeftBound = 1 + 2 * VertexUpperBound + currentBound;
                destEdgeBounds[2 * s] = finalLeftBound;
                destEdgeBounds[2 * s + 1] = currentOutEdges.Length;
                currentBound += currentOutEdges.Length;
                if (outEdges[s].Buffer != null)
                    ArrayPool<int>.Shared.Return(outEdges[s].Buffer);
            }

            // TODO: Clear on return to pool.
            Array.Clear(OutEdges, 0, OutEdges.Length);
            OutEdges = ArrayBuilder<ArrayBuilder<int>>.EmptyArray;

            Span<int> destTargets = storage.AsSpan(1 + 2 * VertexUpperBound + _sources.Count, _targets.Count);
            _targets.AsSpan().CopyTo(destTargets);
            if (_targets.Buffer != null)
                ArrayPool<int>.Shared.Return(_targets.Buffer);
            _targets = default;

            Span<int> destSources = storage.AsSpan(1 + 2 * VertexUpperBound + _sources.Count + _targets.Count,
                _sources.Count);
            _sources.AsSpan().CopyTo(destSources);
            if (_sources.Buffer != null)
                ArrayPool<int>.Shared.Return(_sources.Buffer);
            _sources = default;

            VertexUpperBound = 0;

            return new AdjacencyListIncidenceGraph(storage);
        }
    }
}
