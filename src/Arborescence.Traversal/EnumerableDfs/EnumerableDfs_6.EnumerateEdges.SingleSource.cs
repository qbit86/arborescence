namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TExploredSetPolicy>
    {
        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <returns>An enumerator to enumerate the edges of the the graph.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>.
        /// </exception>
        public IEnumerator<TEdge> EnumerateEdges(TGraph graph, TVertex source, TExploredSet exploredSet)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            var stack = new Internal.Stack<TEdgeEnumerator>();
            try
            {
                ExploredSetPolicy.Add(exploredSet, source);
                stack.Add(graph.EnumerateOutEdges(source));

                while (stack.TryTake(out TEdgeEnumerator outEdges))
                {
                    if (!outEdges.MoveNext())
                        continue;

                    stack.Add(outEdges);

                    TEdge e = outEdges.Current;
                    if (!graph.TryGetHead(e, out TVertex v))
                        continue;

                    if (ExploredSetPolicy.Contains(exploredSet, v))
                        continue;

                    yield return e;
                    ExploredSetPolicy.Add(exploredSet, v);
                    stack.Add(graph.EnumerateOutEdges(v));
                }
            }
            finally
            {
                stack.Dispose();
            }
        }
    }
}
