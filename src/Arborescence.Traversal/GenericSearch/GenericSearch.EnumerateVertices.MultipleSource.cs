namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

#if DEBUG
    using System.Diagnostics;

#endif

    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe,
        TExploredSet, TFringePolicy, TExploredSetPolicy>
    {
        /// <summary>
        /// Enumerates vertices of the graph in an order specified by the fringe starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="fringe">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
        /// <returns>An enumerator to enumerate the vertices of the the graph.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        public IEnumerator<TVertex> EnumerateVertices<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator sources, TFringe fringe, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            while (sources.MoveNext())
            {
                TVertex source = sources.Current;
                ExploredSetPolicy.Add(exploredSet, source);
                yield return source;
                FringePolicy.Add(fringe, source);
            }

            while (FringePolicy.TryTake(fringe, out TVertex u))
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

                    ExploredSetPolicy.Add(exploredSet, v);
                    yield return v;
                    FringePolicy.Add(fringe, v);
                }
            }
        }
    }
}
