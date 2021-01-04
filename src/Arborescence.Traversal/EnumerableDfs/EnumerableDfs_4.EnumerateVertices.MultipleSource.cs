namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct EnumerableDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
    {
        /// <summary>
        /// Enumerates vertices of the graph in a depth-first order starting from the multiple sources.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="sources">The sources enumerator.</param>
        /// <param name="exploredSet">The set of explored vertices.</param>
        /// <typeparam name="TVertexEnumerator">The type of the vertex enumerator.</typeparam>
        /// <typeparam name="TExploredSet">The type of the set of explored vertices.</typeparam>
        /// <returns>An enumerator to enumerate the vertices of a depth-first search tree.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graph"/> is <see langword="null"/>,
        /// or <paramref name="sources"/> is <see langword="null"/>.
        /// </exception>
        public IEnumerator<TVertex> EnumerateVertices<TVertexEnumerator, TExploredSet>(
            TGraph graph, TVertexEnumerator sources, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            var stack = new Internal.Stack<TEdgeEnumerator>();
            try
            {
                while (sources.MoveNext())
                {
                    TVertex source = sources.Current;
                    if (exploredSet.Contains(source))
                        continue;

                    exploredSet.Add(source);
                    yield return source;
                    stack.Add(graph.EnumerateOutEdges(source));

                    while (stack.TryTake(out TEdgeEnumerator outEdges))
                    {
                        if (!outEdges.MoveNext())
                            continue;

                        stack.Add(outEdges);

                        TEdge e = outEdges.Current;
                        if (!graph.TryGetHead(e, out TVertex v))
                            continue;

                        if (exploredSet.Contains(v))
                            continue;

                        exploredSet.Add(v);
                        yield return v;
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
