namespace Arborescence.Traversal.Specialized
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct EnumerableBfs<TGraph, TEdge, TEdgeEnumerator>
    {
        public IEnumerator<TEdge> EnumerateEdges(TGraph graph, int vertexCount, int source)
        {
            var queue = new Internal.Queue<int>();
            try
            {
                throw new NotImplementedException();
            }
            finally
            {
                // The Dispose call will happen on the original value of the local if it is the argument to a using statement.
                queue.Dispose();
            }
        }
    }
}
