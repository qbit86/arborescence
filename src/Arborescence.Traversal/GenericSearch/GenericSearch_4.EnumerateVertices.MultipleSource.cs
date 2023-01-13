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
        /// Enumerates vertices of the graph in an order specified by the frontier starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="frontier">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
        /// <typeparam name="TFrontier">The type of the generic queue.</typeparam>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerator to enumerate the vertices of a search tree.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>,
        /// or <paramref name="frontier"/> is <see langword="null"/>,
        /// or <paramref name="exploredSet"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <see cref="IProducerConsumerCollection{TVertex}.TryAdd"/> for <paramref name="frontier"/>
        /// returns <see langword="false"/>.
        /// </exception>
        public IEnumerator<TVertex> EnumerateVertices<TVertexEnumerator, TFrontier, TExploredSet>(
            TGraph graph, TVertexEnumerator sources, TFrontier frontier, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (sources is null)
                ThrowHelper.ThrowArgumentNullException(nameof(sources));

            if (frontier is null)
                ThrowHelper.ThrowArgumentNullException(nameof(frontier));

            if (exploredSet is null)
                ThrowHelper.ThrowArgumentNullException(nameof(exploredSet));

            return EnumerateVerticesIterator(graph, sources, frontier, exploredSet);
        }

        private static IEnumerator<TVertex> EnumerateVerticesIterator<TSourceEnumerator, TFrontier, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TFrontier frontier, TExploredSet exploredSet)
            where TSourceEnumerator : IEnumerator<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            while (sources.MoveNext())
            {
                TVertex source = sources.Current;
                exploredSet.Add(source);
                yield return source;
                frontier.AddOrThrow(source);
            }

            while (frontier.TryTake(out TVertex? current))
            {
#if DEBUG
                Debug.Assert(exploredSet.Contains(current));
#endif
                TEdgeEnumerator outEdges = graph.EnumerateOutEdges(current);
                try
                {
                    while (outEdges.MoveNext())
                    {
                        if (!graph.TryGetHead(outEdges.Current, out TVertex? neighbor))
                            continue;

                        if (exploredSet.Contains(neighbor))
                            continue;

                        exploredSet.Add(neighbor);
                        yield return neighbor;
                        frontier.AddOrThrow(neighbor);
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
