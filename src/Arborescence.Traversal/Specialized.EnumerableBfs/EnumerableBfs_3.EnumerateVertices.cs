namespace Arborescence.Traversal.Specialized
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
#if DEBUG
    using System.Diagnostics;
#endif

    public readonly partial struct EnumerableBfs<TGraph, TEdge, TEdgeEnumerator>
    {
        /// <summary>
        /// Enumerates vertices of the graph in a breadth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <returns>An enumerator to enumerate the vertices of a depth-first search tree.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero.
        /// </exception>
        public IEnumerator<int> EnumerateVertices(TGraph graph, int source, int vertexCount)
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            return EnumerateVerticesIterator(graph, source, vertexCount);
        }

        private static IEnumerator<int> EnumerateVerticesIterator(TGraph graph, int source, int vertexCount)
        {
            if (unchecked((uint)source >= vertexCount))
            {
                yield return source;
                yield break;
            }

            byte[] exploredSet = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(exploredSet, 0, exploredSet.Length);
            var queue = new Internal.Queue<int>();
            try
            {
                SetHelpers.Add(exploredSet, source);
                yield return source;
                queue.Add(source);

                while (queue.TryTake(out int u))
                {
#if DEBUG
                    Debug.Assert(SetHelpers.Contains(exploredSet, u));
#endif
                    TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                    try
                    {
                        while (outEdges.MoveNext())
                        {
                            TEdge e = outEdges.Current;
                            if (!graph.TryGetHead(e, out int v))
                                continue;

                            if (SetHelpers.Contains(exploredSet, v))
                                continue;

                            SetHelpers.Add(exploredSet, v);
                            yield return v;
                            queue.Add(v);
                        }
                    }
                    finally
                    {
                        outEdges.Dispose();
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
