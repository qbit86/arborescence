namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    public readonly partial struct EnumerableBfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TExploredSetPolicy>
    {
        /// <summary>
        /// Enumerates edges of the graph in a breadth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
        /// <returns>An enumerator to enumerate the edges of the the graph.</returns>
        public IEnumerator<TEdge> EnumerateEdges<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator sources, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            var queue = new Internal.Queue<TVertex>();
            try
            {
                while (sources.MoveNext())
                {
                    TVertex source = sources.Current;
                    ExploredSetPolicy.Add(exploredSet, source);
                    queue.Add(source);
                }

                while (queue.TryTake(out TVertex u))
                {
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
