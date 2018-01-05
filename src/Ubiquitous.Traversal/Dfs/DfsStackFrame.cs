namespace Ubiquitous
{
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
