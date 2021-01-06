namespace Arborescence.Traversal
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
#if DEBUG
    using System.Diagnostics;

#endif

    public readonly partial struct Bfs<TGraph, TEdge, TEdgeEnumerator>
    {
        /// <summary>
        /// Enumerates edges of the graph in a breadth-first order.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <returns>An enumerator to enumerate the edges of a depth-first search tree.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
        public IEnumerator<TEdge> EnumerateEdges(TGraph graph, int source, int vertexCount)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if (unchecked((uint)source >= (uint)vertexCount))
                yield break;

            byte[] exploredSet = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(exploredSet, 0, exploredSet.Length);
            var queue = new Internal.Queue<int>();
            try
            {
                SetHelpers.Add(exploredSet, source);
                queue.Add(source);

                while (queue.TryTake(out int u))
                {
#if DEBUG
                    Debug.Assert(SetHelpers.Contains(exploredSet, u));
#endif
                    TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;
                        if (!graph.TryGetHead(e, out int v))
                            continue;

                        if (SetHelpers.Contains(exploredSet, v))
                            continue;

                        yield return e;
                        SetHelpers.Add(exploredSet, v);
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
