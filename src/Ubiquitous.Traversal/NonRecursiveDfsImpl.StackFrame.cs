namespace Ubiquitous
{
    internal partial struct NonRecursiveDfsImpl<TGraph, TVertex, TEdge, TEdges, TColorMap,
        TVertexConcept, TEdgeConcept>
    {
        internal enum StackFrameKind
        {
            None = 0,
            ProcessVertexPrologue,
            ProcessVertexEpilogue,
            ProcessEdgePrologue,
            ProcessEdgeEpilogue,
        }

        internal struct StackFrame
        {
            internal StackFrameKind Kind { get; }

            internal TVertex Vertex { get; }

            internal StackFrame(StackFrameKind kind, TVertex vertex)
            {
                Kind = kind;
                Vertex = vertex;
            }
        }
    }
}
