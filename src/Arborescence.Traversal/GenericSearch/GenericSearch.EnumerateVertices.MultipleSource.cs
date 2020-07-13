namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TFringe,
        TExploredSet, TGraphPolicy, TFringePolicy, TExploredSetPolicy>
    {
        /// <summary>
        /// Enumerates vertices of the graph in a depth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="fringe">The collection of discovered vertices which are not finished yet.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
        /// <returns>An enumerator to enumerate the vertices of the the graph.</returns>
        public IEnumerator<TVertex> EnumerateVertices<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator sources, TFringe fringe, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            while (sources.MoveNext())
            {
                TVertex source = sources.Current;
                FringePolicy.Add(fringe, source);
            }

            while (FringePolicy.TryTake(fringe, out TVertex u))
            {
                if (ExploredSetPolicy.Contains(exploredSet, u))
                    continue;

                ExploredSetPolicy.Add(exploredSet, u);
                yield return u;

                TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(graph, u);
                while (outEdges.MoveNext())
                {
                    TEdge e = outEdges.Current;
                    if (!GraphPolicy.TryGetHead(graph, e, out TVertex v))
                        continue;

                    FringePolicy.Add(fringe, v);
                }
            }
        }
    }
    // ReSharper restore UnusedTypeParameter
}
