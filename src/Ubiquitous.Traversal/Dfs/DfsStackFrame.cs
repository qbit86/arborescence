namespace Ubiquitous
{
    internal static class DfsStackFrame
    {
        public static DfsStackFrame<TVertex, TEdge, TEdgeEnumerator> Create<TVertex, TEdge, TEdgeEnumerator>(
            TVertex vertex, bool hasEdge, TEdge edge, TEdgeEnumerator edgeEnumerator)
        {
            return new DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>(vertex, hasEdge, edge, edgeEnumerator);
        }
    }

    public struct DfsStackFrame<TVertex, TEdge, TEdgeEnumerator>
    {
        internal TVertex Vertex { get; }

        internal bool HasEdge { get; }

        internal TEdge Edge { get; }

        internal TEdgeEnumerator EdgeEnumerator { get; }

        internal DfsStackFrame(TVertex vertex, bool hasEdge, TEdge edge, TEdgeEnumerator edgeEnumerator)
        {
            Vertex = vertex;
            HasEdge = hasEdge;
            Edge = edge;
            EdgeEnumerator = edgeEnumerator;
        }
    }
}
