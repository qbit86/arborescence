namespace Arborescence.Models
{
    using System;

    public sealed class MutableSimpleIncidenceGraph :
        IIncidenceGraph<int, Endpoints, ArrayPrefixEnumerator<Endpoints>>,
        IGraphBuilder<SimpleIncidenceGraph, int, Endpoints>,
        IDisposable
    {
        private const int DefaultInitialOutDegree = 4;

        private int _initialOutDegree = DefaultInitialOutDegree;
        private ArrayPrefix<ArrayPrefix<Endpoints>> _outEdgesByVertex;

        public MutableSimpleIncidenceGraph(int initialVertexCount)
        {
            if (initialVertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(initialVertexCount));

            _outEdgesByVertex = ArrayPrefixBuilder.Resize(_outEdgesByVertex, initialVertexCount, true);
        }

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        public int VertexCount => _outEdgesByVertex.Count;

        public void Dispose() => throw new NotImplementedException();

        public bool TryAdd(int tail, int head, out Endpoints edge) => throw new NotImplementedException();

        public SimpleIncidenceGraph ToGraph() => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool TryGetHead(Endpoints edge, out int head)
        {
            head = edge.Head;
            return unchecked((uint)head < (uint)VertexCount);
        }

        /// <inheritdoc/>
        public bool TryGetTail(Endpoints edge, out int tail)
        {
            tail = edge.Tail;
            return unchecked((uint)tail < (uint)VertexCount);
        }

        /// <inheritdoc/>
        public ArrayPrefixEnumerator<Endpoints> EnumerateOutEdges(int vertex)
        {
            if (unchecked((uint)vertex >= (uint)_outEdgesByVertex.Count))
                return ArrayPrefixEnumerator<Endpoints>.Empty;

            ArrayPrefix<Endpoints> outEdges = _outEdgesByVertex[vertex];
            return new ArrayPrefixEnumerator<Endpoints>(outEdges.Array ?? Array.Empty<Endpoints>(), outEdges.Count);
        }
    }
}
