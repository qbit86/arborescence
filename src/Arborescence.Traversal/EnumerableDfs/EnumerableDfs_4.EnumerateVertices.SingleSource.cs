namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct EnumerableDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
    {
        public IEnumerator<TVertex> EnumerateVertices<TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TExploredSet : ISet<TVertex>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            var stack = new Internal.Stack<TEdgeEnumerator>();
            try
            {
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
            finally
            {
                stack.Dispose();
            }
        }
    }
}
