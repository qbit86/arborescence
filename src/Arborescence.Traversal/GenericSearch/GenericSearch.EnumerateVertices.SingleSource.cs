namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe,
        TExploredSet, TFringePolicy, TExploredSetPolicy>
    {
        /// <summary>
        /// Enumerates vertices of the graph in an order specified by the fringe starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="fringe">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <returns>An enumerator to enumerate the vertices of the the graph.</returns>
        public IEnumerator<TVertex> EnumerateVertices(
            TGraph graph, TVertex source, TFringe fringe, TExploredSet exploredSet)
        {
            FringePolicy.Add(fringe, source);

            while (FringePolicy.TryTake(fringe, out TVertex u))
            {
                if (ExploredSetPolicy.Contains(exploredSet, u))
                    continue;

                ExploredSetPolicy.Add(exploredSet, u);
                yield return u;

                TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                while (outEdges.MoveNext())
                {
                    TEdge e = outEdges.Current;
                    if (!graph.TryGetHead(e, out TVertex v))
                        continue;

                    FringePolicy.Add(fringe, v);
                }
            }
        }
    }
}
