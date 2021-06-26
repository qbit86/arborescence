namespace Arborescence.Traversal
{
    internal readonly struct StackFrame<TVertex, TEdge, TEdgeEnumerator>
    {
        private readonly TEdge _edge;
        private readonly bool _hasEdge;

        internal StackFrame(TVertex vertex, TEdgeEnumerator edgeEnumerator)
        {
            _hasEdge = false;
            _edge = default!;
            Vertex = vertex;
            EdgeEnumerator = edgeEnumerator;
        }

        internal StackFrame(TVertex vertex, TEdge edge, TEdgeEnumerator edgeEnumerator)
        {
            _hasEdge = true;
            _edge = edge;
            Vertex = vertex;
            EdgeEnumerator = edgeEnumerator;
        }

        internal TVertex Vertex { get; }
        internal TEdgeEnumerator EdgeEnumerator { get; }

        internal bool TryGetEdge(out TEdge edge)
        {
            edge = _edge;
            return _hasEdge;
        }
    }
}
