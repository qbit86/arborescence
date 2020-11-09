namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    public readonly partial struct ReverseDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TExploredSetPolicy>
    {
        /// <summary>
        /// Enumerates vertices of the graph in a depth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <returns>An enumerator to enumerate the vertices of the the graph.</returns>
        public IEnumerator<TVertex> EnumerateVertices(TGraph graph, TVertex source, TExploredSet exploredSet)
        {
            var stack = new Internal.Stack<TVertex>();
            try
            {
                stack.Add(source);

                while (stack.TryTake(out TVertex u))
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

                        stack.Add(v);
                    }
                }
            }
            finally
            {
                stack.Dispose();
            }
        }
    }
}
