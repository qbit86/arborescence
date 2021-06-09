namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct EnumerableDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
    {
        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerator to enumerate the edges of a depth-first search tree.</returns>
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
            var stack = new Internal.Stack<TEdgeEnumerator>();
            try
            {
                exploredSet.Add(source);
                stack.Add(graph.EnumerateOutEdges(source));

                while (stack.TryTake(out TEdgeEnumerator outEdges))
                {
                    if (!outEdges.MoveNext())
                    {
                        outEdges.Dispose();
                        continue;
                    }

                    stack.Add(outEdges);

                    TEdge e = outEdges.Current;
                    if (!graph.TryGetHead(e, out TVertex v))
                        continue;

                    if (exploredSet.Contains(v))
                        continue;

                    yield return e;
                    exploredSet.Add(v);
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
