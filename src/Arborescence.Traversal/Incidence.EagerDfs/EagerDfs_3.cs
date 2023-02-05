namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the DFS algorithm — depth-first traversal of the graph in a non-recursive manner.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertex.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TEdgeEnumerator">The type of the edge enumerator.</typeparam>
    public static partial class EagerDfs<TVertex, TEdge, TEdgeEnumerator>
        where TVertex : notnull
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        private readonly struct StackFrame
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
}
