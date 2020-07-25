namespace Arborescence.Models
{
    using System;

    public sealed class MutableIndexedIncidenceGraph :
        IIncidenceGraph<int, int, ArrayPrefixEnumerator<int>>
    {
        private const int DefaultInitialOutDegree = 4;

        private int _initialOutDegree = DefaultInitialOutDegree;
        private ArrayPrefix<int> _tailByEdge;
        private ArrayPrefix<int> _headByEdge;
        private ArrayPrefix<ArrayPrefix<int>> _outEdgesByVertex;

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
    }
}
