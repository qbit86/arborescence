namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;
#if DEBUG
    using System.Diagnostics;

#endif

    public readonly partial struct EnumerableBfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TExploredSetPolicy>
    {
        /// <summary>
        /// Enumerates edges of the graph in a breadth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <returns>An enumerator to enumerate the edges of a breadth-first search tree.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>.
        /// </exception>
        public IEnumerator<TEdge> EnumerateEdges(TGraph graph, TVertex source, TExploredSet exploredSet)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            var queue = new Internal.Queue<TVertex>();
            try
            {
                ExploredSetPolicy.Add(exploredSet, source);
                queue.Add(source);

                while (queue.TryTake(out TVertex u))
                {
#if DEBUG
                    Debug.Assert(ExploredSetPolicy.Contains(exploredSet, u));
#endif
                    TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;
                        if (!graph.TryGetHead(e, out TVertex v))
                            continue;

                        if (ExploredSetPolicy.Contains(exploredSet, v))
                            continue;

                        yield return e;
                        ExploredSetPolicy.Add(exploredSet, v);
                        queue.Add(v);
                    }
                }
            }
            finally
            {
                // The Dispose call will happen on the original value of the local if it is the argument to a using statement.
                queue.Dispose();
            }
        }
    }
}
