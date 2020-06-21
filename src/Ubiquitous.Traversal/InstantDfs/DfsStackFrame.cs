namespace Ubiquitous.Traversal
{
    internal readonly struct DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>
    {
        private readonly bool _hasEdge;
        private readonly TEdge _edge;

        internal TVertex Vertex { get; }

        internal TEdgeEnumerator EdgeEnumerator { get; }

        internal DfsStackFrame(TVertex vertex, bool hasEdge, TEdge edge, TEdgeEnumerator edgeEnumerator)
        {
            Vertex = vertex;
            _hasEdge = hasEdge;
            _edge = edge;
            EdgeEnumerator = edgeEnumerator;
        }

        internal bool TryGetEdge(out TEdge edge)
        {
            edge = _edge;
            return _hasEdge;
        }
    }
}
