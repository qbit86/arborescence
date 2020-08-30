namespace Arborescence.Traversal.Specialized
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;

    public readonly partial struct EnumerableBfs<TGraph, TEdge, TEdgeEnumerator>
    {
        public IEnumerator<TEdge> EnumerateEdges(TGraph graph, int vertexCount, int source)
        {
            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if ((uint)source >= (uint)vertexCount)
                throw new ArgumentOutOfRangeException(nameof(source));

            byte[] exploredSet = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(exploredSet, 0, exploredSet.Length);
            var queue = new Internal.Queue<int>();
            try
            {
                IndexedSetPolicy exploredSetPolicy = default;
                exploredSetPolicy.Add(exploredSet, source);
                queue.Add(source);

                while (queue.TryTake(out int u))
                {
                    TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;
                        if (!graph.TryGetHead(e, out int v))
                            continue;

                        if (exploredSetPolicy.Contains(exploredSet, v))
                            continue;

                        yield return e;
                        exploredSetPolicy.Add(exploredSet, v);
                        queue.Add(v);
                    }
                }
            }
            finally
            {
                // The Dispose call will happen on the original value of the local if it is the argument to a using statement.
                queue.Dispose();
                ArrayPool<byte>.Shared.Return(exploredSet);
            }
        }
    }
}
