namespace Ubiquitous
{
    using System.Collections.Generic;

    internal partial struct NonRecursiveDfsTraversal<TGraph, TVertex, TEdge, TEdges, TColorMap,
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

            internal IEnumerator<TEdge> EdgeEnumerator { get;}

            internal TEdge Edge { get; }

            internal StackFrame(StackFrameKind kind, TVertex vertex, IEnumerator<TEdge> edgeEnumerator, TEdge edge)
            {
                Kind = kind;
                Vertex = vertex;
                EdgeEnumerator = edgeEnumerator;
                Edge = edge;
            }
        }
    }
}
