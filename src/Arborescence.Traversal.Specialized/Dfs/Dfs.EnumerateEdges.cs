namespace Arborescence.Traversal.Dfs
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;

    public readonly partial struct Dfs<TGraph, TEdge, TEdgeEnumerator>
    {
        public IEnumerator<TEdge> EnumerateEdges(TGraph graph, int vertexCount, int source)
        {
            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if ((uint)source >= (uint)vertexCount)
                throw new ArgumentOutOfRangeException(nameof(source));

            byte[] exploredSet = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(exploredSet, 0, exploredSet.Length);
            var stack = new Internal.Stack<TEdgeEnumerator>();
            try
            {
                throw new NotImplementedException();
            }
            finally
            {
                stack.Dispose();
                ArrayPool<byte>.Shared.Return(exploredSet);
            }
        }
    }
}
