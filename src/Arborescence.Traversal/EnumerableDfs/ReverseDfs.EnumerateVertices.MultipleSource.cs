namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct ReverseDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TExploredSetPolicy>
    {
        /// <summary>
        /// Enumerates vertices of the graph in a depth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
        /// <returns>An enumerator to enumerate the vertices of the the graph.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        public IEnumerator<TVertex> EnumerateVertices<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator sources, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            var stack = new Internal.Stack<TVertex>();
            try
            {
                while (sources.MoveNext())
                {
                    TVertex source = sources.Current;
                    stack.Add(source);
                }

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
