namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        // https://11011110.github.io/blog/2013/12/17/stack-based-graph-traversal.html

        /// <summary>
        /// Enumerates vertices of the graph in a depth-first order starting from the single source.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <returns>An enumerator to enumerate the vertices of the the graph.</returns>
        public IEnumerator<TVertex> EnumerateVertices(TGraph graph, TVertex source, TExploredSet exploredSet)
        {
            var stack = new Internal.Stack<TEdgeEnumerator>();
            try
            {
                ExploredSetPolicy.Add(exploredSet, source);
                yield return source;
                stack.Add(GraphPolicy.EnumerateOutEdges(graph, source));

                while (stack.TryTake(out TEdgeEnumerator outEdges))
                {
                    if (!outEdges.MoveNext())
                        continue;

                    stack.Add(outEdges);

                    TEdge e = outEdges.Current;
                    if (!GraphPolicy.TryGetHead(graph, e, out TVertex v))
                        continue;

                    if (ExploredSetPolicy.Contains(exploredSet, v))
                        continue;

                    ExploredSetPolicy.Add(exploredSet, v);
                    yield return v;
                    stack.Add(GraphPolicy.EnumerateOutEdges(graph, v));
                }
            }
            finally
            {
                stack.Dispose();
            }
        }
    }
}
