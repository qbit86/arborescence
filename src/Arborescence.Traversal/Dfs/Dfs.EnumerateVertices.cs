namespace Arborescence.Traversal
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;

    public readonly partial struct Dfs<TGraph, TEdge, TEdgeEnumerator>
    {
        /// <summary>
        /// Enumerates vertices of the graph in a depth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="vertexCount">The number of vertices.</param>
        /// <param name="source">The source.</param>
        /// <returns>An enumerator to enumerate the vertices of the the graph.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="vertexCount"/> is less than zero,
        /// or <paramref name="source"/> is greater than or equal to <paramref name="vertexCount"/>.
        /// </exception>
        public IEnumerator<int> EnumerateVertices(TGraph graph, int vertexCount, int source)
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
                yield return source;
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

                    SetHelpers.Add(exploredSet, v);
                    yield return v;
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
