namespace Arborescence.Traversal
{
    using System.Collections.Generic;

    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        /// <summary>
        /// Enumerates vertices of the graph in a depth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
        /// <returns>An enumerator to enumerate the vertices of the the graph.</returns>
        public IEnumerator<TVertex> EnumerateVertices<TVertexEnumerator>(
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
            }
            finally
            {
                stack.Dispose();
            }
        }
    }
}
