namespace Arborescence.Models
{
    using System;
    using System.Buffers;
    using static System.Diagnostics.Debug;

    internal struct JaggedAdjacencyListIncidenceGraphBuilder :
        IGraphBuilder<JaggedAdjacencyListIncidenceGraph, int, int>
    {
        private const int DefaultInitialOutDegree = 4;

        private int _initialOutDegree;
        private ArrayBuilder<int> _tails;
        private ArrayBuilder<int> _heads;
        private ArrayPrefix<ArrayPrefix<int>> _outEdges;

        public JaggedAdjacencyListIncidenceGraphBuilder(int initialVertexCount) : this(initialVertexCount, 0) { }

        public JaggedAdjacencyListIncidenceGraphBuilder(int initialVertexCount, int edgeCapacity)
        {
            if (initialVertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

            if (edgeCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCapacity));

            _initialOutDegree = DefaultInitialOutDegree;
            int effectiveEdgeCapacity = Math.Max(edgeCapacity, DefaultInitialOutDegree);
            _tails = new ArrayBuilder<int>(effectiveEdgeCapacity);
            _heads = new ArrayBuilder<int>(effectiveEdgeCapacity);
            ArrayPrefix<int>[] outEdges = ArrayPool<ArrayPrefix<int>>.Shared.Rent(initialVertexCount);
            Array.Clear(outEdges, 0, initialVertexCount);
            _outEdges = ArrayPrefix.Create(outEdges, initialVertexCount);
        }

        private static ArrayPool<int> Pool => ArrayPool<int>.Shared;

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

        public bool TryAdd(int tail, int head, out int edge)
        {
            if (tail < 0)
            {
                edge = -1;
                return false;
            }

            if (head < 0)
            {
                edge = -2;
                return false;
            }

            int max = Math.Max(tail, head);
            EnsureVertexCount(max + 1);

            Assert(_tails.Count == _heads.Count);
            int newEdgeIndex = _heads.Count;
            _tails.Add(tail);
            _heads.Add(head);

            if (_outEdges[tail].Array is null)
                _outEdges[tail] = ArrayPrefix.Create(Pool.Rent(InitialOutDegree), 0);

            _outEdges[tail] = ArrayPrefixBuilder.Add(_outEdges[tail], newEdgeIndex, true);

            edge = newEdgeIndex;
            return true;
        }

        public JaggedAdjacencyListIncidenceGraph ToGraph()
        {
            Assert(_tails.Count == _heads.Count);
            ReadOnlySpan<int> headsBuffer = new Span<int>(_heads.Buffer, 0, _heads.Count);
            ReadOnlySpan<int> tailsBuffer = new Span<int>(_tails.Buffer, 0, _tails.Count);
            int[] endpoints = _heads.Count > 0 ? new int[_heads.Count * 2] : ArrayBuilder<int>.EmptyArray;
            if (endpoints.Length > 0)
            {
                headsBuffer.CopyTo(endpoints.AsSpan(0, _heads.Count));
                tailsBuffer.CopyTo(endpoints.AsSpan(_heads.Count, _tails.Count));
            }

            var outEdges = new ArrayPrefix<int>[_outEdges.Count];
            _outEdges.CopyTo(outEdges);

            if (_outEdges.Array != null)
                ArrayPool<ArrayPrefix<int>>.Shared.Return(_outEdges.Array, true);
            _outEdges = ArrayPrefix<ArrayPrefix<int>>.Empty;

            _tails.Dispose(false);
            _heads.Dispose(false);

            return new JaggedAdjacencyListIncidenceGraph(endpoints, outEdges);
        }
    }
}
