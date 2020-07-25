namespace Arborescence.Models
{
    using System;
    using System.Diagnostics;

    public sealed class MutableIndexedIncidenceGraph :
        IIncidenceGraph<int, int, ArrayPrefixEnumerator<int>>,
        IGraphBuilder<IndexedIncidenceGraph, int, int>
    {
        private const int DefaultInitialOutDegree = 4;

        private ArrayPrefix<int> _headByEdge;
        private int _initialOutDegree = DefaultInitialOutDegree;
        private ArrayPrefix<ArrayPrefix<int>> _outEdgesByVertex;
        private ArrayPrefix<int> _tailByEdge;

        public MutableIndexedIncidenceGraph(int initialVertexCount, int edgeCapacity = 0)
        {
            if (initialVertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

            if (edgeCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(edgeCapacity));

            int effectiveEdgeCapacity = Math.Max(edgeCapacity, DefaultInitialOutDegree);
            _tailByEdge = ArrayPrefixBuilder.Create<int>(effectiveEdgeCapacity);
            _headByEdge = ArrayPrefixBuilder.Create<int>(effectiveEdgeCapacity);
            _outEdgesByVertex = ArrayPrefixBuilder.Create<ArrayPrefix<int>>(initialVertexCount);
        }

        public int VertexCount => _outEdgesByVertex.Count;

        public int InitialOutDegree
        {
            get => _initialOutDegree <= 0 ? DefaultInitialOutDegree : _initialOutDegree;
            set => _initialOutDegree = value;
        }

        public bool TryAdd(int tail, int head, out int edge)
        {
            if (tail < 0)
            {
                edge = default;
                return false;
            }

            if (head < 0)
            {
                edge = default;
                return false;
            }

            int max = Math.Max(tail, head);
            EnsureVertexCount(max + 1);

            Debug.Assert(_tailByEdge.Count == _headByEdge.Count, "_tailByEdge.Count == _headByEdge.Count");
            int newEdgeIndex = _headByEdge.Count;
            _tailByEdge = ArrayPrefixBuilder.Add(_tailByEdge, tail, false);
            _headByEdge = ArrayPrefixBuilder.Add(_headByEdge, head, false);

            if (_outEdgesByVertex[tail].Array is null)
                _outEdgesByVertex[tail] = ArrayPrefixBuilder.Create<int>(InitialOutDegree);
            _outEdgesByVertex[tail] = ArrayPrefixBuilder.Add(_outEdgesByVertex[tail], newEdgeIndex, false);

            edge = newEdgeIndex;
            return true;
        }

        public IndexedIncidenceGraph ToGraph() => throw new NotImplementedException();

        public bool TryGetHead(int edge, out int head)
        {
            if (unchecked((uint)edge > (uint)_headByEdge.Count))
            {
                head = default;
                return false;
            }

            head = _headByEdge[edge];
            return true;
        }

        public bool TryGetTail(int edge, out int tail)
        {
            if (unchecked((uint)edge > (uint)_tailByEdge.Count))
            {
                tail = default;
                return false;
            }

            tail = _tailByEdge[edge];
            return true;
        }

        public ArrayPrefixEnumerator<int> EnumerateOutEdges(int vertex)
        {
            if (unchecked((uint)vertex > (uint)_outEdgesByVertex.Count))
                return new ArrayPrefixEnumerator<int>(Array.Empty<int>(), 0);

            ArrayPrefix<int> outEdges = _outEdgesByVertex[vertex];
            return new ArrayPrefixEnumerator<int>(outEdges.Array, outEdges.Count);
        }

        public void EnsureVertexCount(int vertexCount)
        {
            if (vertexCount > VertexCount)
                _outEdgesByVertex = ArrayPrefixBuilder.Resize(_outEdgesByVertex, vertexCount, false);
        }
    }
}
