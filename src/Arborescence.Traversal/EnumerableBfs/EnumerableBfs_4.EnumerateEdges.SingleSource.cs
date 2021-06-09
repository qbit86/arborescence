namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;
#if DEBUG
    using Debug = System.Diagnostics.Debug;

#endif

    public readonly partial struct EnumerableBfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
    {
        /// <summary>
        /// Enumerates edges of the graph in a breadth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerator to enumerate the edges of a breadth-first search tree.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="exploredSet"/> is <see langword="null"/>.
        /// </exception>
        public IEnumerator<TEdge> EnumerateEdges<TExploredSet>(TGraph graph, TVertex source, TExploredSet exploredSet)
            where TExploredSet : ISet<TVertex>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (exploredSet == null)
                throw new ArgumentNullException(nameof(exploredSet));

            return EnumerateEdgesIterator(graph, source, exploredSet);
        }

        private static IEnumerator<TEdge> EnumerateEdgesIterator<TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TExploredSet : ISet<TVertex>
        {
            var queue = new Internal.Queue<TVertex>();
            try
            {
                exploredSet.Add(source);
                queue.Add(source);

                while (queue.TryTake(out TVertex u))
                {
#if DEBUG
                    Debug.Assert(exploredSet.Contains(u));
#endif
                    TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                    try
                    {
                        while (outEdges.MoveNext())
                        {
                            TEdge e = outEdges.Current;
                            if (!graph.TryGetHead(e, out TVertex v))
                                continue;

                            if (exploredSet.Contains(v))
                                continue;

                            yield return e;
                            exploredSet.Add(v);
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
            }
        }
    }
}
