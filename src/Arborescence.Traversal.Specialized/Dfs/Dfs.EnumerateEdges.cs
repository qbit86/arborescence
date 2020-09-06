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
                SetHelpers.Add(exploredSet, source);
                stack.Add(graph.EnumerateOutEdges(source));

                while (stack.TryTake(out TEdgeEnumerator outEdges))
                {
                    if (!outEdges.MoveNext())
                        continue;

                    stack.Add(outEdges);

                    TEdge e = outEdges.Current;
                    if (!graph.TryGetHead(e, out int v))
                        continue;

                    if (SetHelpers.Contains(exploredSet, v))
                        continue;

                    yield return e;
                    SetHelpers.Add(exploredSet, v);
                    stack.Add(graph.EnumerateOutEdges(v));
                }
            }
            finally
            {
                stack.Dispose();
                ArrayPool<byte>.Shared.Return(exploredSet);
            }
        }
    }
}
