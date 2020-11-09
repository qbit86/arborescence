namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TExploredSetPolicy>
    {
        /// <summary>
        /// Enumerates edges of the graph in a depth-first order starting from the multiple sources.
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
            var stack = new Internal.Stack<TEdgeEnumerator>();
            try
            {
                while (sources.MoveNext())
                {
                    TVertex source = sources.Current;
                    if (ExploredSetPolicy.Contains(exploredSet, source))
                        continue;

                    ExploredSetPolicy.Add(exploredSet, source);
                    stack.Add(graph.EnumerateOutEdges(source));

                    while (stack.TryTake(out TEdgeEnumerator outEdges))
                    {
                        if (!outEdges.MoveNext())
                            continue;

                        stack.Add(outEdges);

                        TEdge e = outEdges.Current;
                        if (!graph.TryGetHead(e, out TVertex v))
                            continue;

                        if (ExploredSetPolicy.Contains(exploredSet, v))
                            continue;

                        yield return e;
                        ExploredSetPolicy.Add(exploredSet, v);
                        stack.Add(graph.EnumerateOutEdges(v));
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
