namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
#if DEBUG
    using Debug = System.Diagnostics.Debug;

#endif

    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator>
    {
        /// <summary>
        /// Enumerates vertices of the graph in an order specified by the frontier starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TFrontier">The type of the generic queue.</typeparam>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerator to enumerate the vertices of a search tree.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="frontier"/> is <see langword="null"/>,
        /// or <paramref name="exploredSet"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <see cref="IProducerConsumerCollection{TVertex}.TryAdd"/> for <paramref name="frontier"/>
        /// returns <see langword="false"/>.
        /// </exception>
        public IEnumerator<TVertex> EnumerateVertices<TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (frontier is null)
                ThrowHelper.ThrowArgumentNullException(nameof(frontier));

            if (exploredSet is null)
                ThrowHelper.ThrowArgumentNullException(nameof(exploredSet));

            return EnumerateVerticesIterator(graph, source, frontier, exploredSet);
        }

        private static IEnumerator<TVertex> EnumerateVerticesIterator<TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            exploredSet.Add(source);
            yield return source;
            if (!frontier.TryAdd(source))
                throw new InvalidOperationException(nameof(frontier.TryAdd));

            while (frontier.TryTake(out TVertex? u))
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
                        if (!graph.TryGetHead(e, out TVertex? v))
                            continue;

                        if (exploredSet.Contains(v))
                            continue;

                        exploredSet.Add(v);
                        yield return v;
                        if (!frontier.TryAdd(v))
                            throw new InvalidOperationException(nameof(frontier.TryAdd));
                    }
                }
                finally
                {
                    outEdges.Dispose();
                }
            }
        }
    }
}
