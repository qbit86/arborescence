namespace Arborescence.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly partial struct EnumerableDfs<TGraph, TVertex, TEdge, TEdgeEnumerator>
    {
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
