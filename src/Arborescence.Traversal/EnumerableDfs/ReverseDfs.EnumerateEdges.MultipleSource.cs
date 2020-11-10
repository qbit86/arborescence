namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct ReverseDfs<
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
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        public IEnumerator<TEdge> EnumerateEdges<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator sources, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            var stack = new Internal.Stack<EdgeInfo<TVertex, TEdge>>();
            try
            {
                while (sources.MoveNext())
                {
                    TVertex source = sources.Current;
                    stack.Add(new EdgeInfo<TVertex, TEdge>(source));
                }

                while (stack.TryTake(out EdgeInfo<TVertex, TEdge> stackFrame))
                {
                    TVertex u = stackFrame.ExploredVertex;
                    if (ExploredSetPolicy.Contains(exploredSet, u))
                        continue;

                    if (stackFrame.TryGetInEdge(out TEdge inEdge))
                        yield return inEdge;
                    ExploredSetPolicy.Add(exploredSet, u);

                    TEdgeEnumerator outEdges = graph.EnumerateOutEdges(u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;
                        if (!graph.TryGetHead(e, out TVertex v))
                            continue;

                        stack.Add(new EdgeInfo<TVertex, TEdge>(v, e));
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
