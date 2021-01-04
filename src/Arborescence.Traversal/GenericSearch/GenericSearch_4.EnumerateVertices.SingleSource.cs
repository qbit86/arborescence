﻿namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;

    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator>
    {
        /// <summary>
        /// Enumerates vertices of the graph in an order specified by the fringe starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="fringe">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TFringe">The type of the generic queue.</typeparam>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerator to enumerate the vertices of a search tree.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="graph"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// <see cref="IProducerConsumerCollection{TVertex}.TryAdd"/> for <paramref name="fringe"/>
        /// returns <see langword="false"/>.
        /// </exception>
        public IEnumerator<TVertex> EnumerateVertices<TFringe, TExploredSet>(
            TGraph graph, TVertex source, TFringe fringe, TExploredSet exploredSet)
            where TFringe : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            exploredSet.Add(source);
            yield return source;
            if (!fringe.TryAdd(source))
                throw new InvalidOperationException(nameof(fringe.TryAdd));

            while (fringe.TryTake(out TVertex u))
            {
#if DEBUG
                Debug.Assert(exploredSet.Contains(u));
#endif
                TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                while (outEdges.MoveNext())
                {
                    TEdge e = outEdges.Current;
                    if (!graph.TryGetHead(e, out TVertex v))
                        continue;

                    if (exploredSet.Contains(v))
                        continue;

                    exploredSet.Add(v);
                    yield return v;
                    if (!fringe.TryAdd(v))
                        throw new InvalidOperationException(nameof(fringe.TryAdd));
                }
            }
        }
    }
}