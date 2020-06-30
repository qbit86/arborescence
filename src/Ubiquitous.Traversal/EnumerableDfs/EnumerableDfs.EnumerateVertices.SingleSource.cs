namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
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

                    TEdge e = outEdges.Current;
                    if (!GraphPolicy.TryGetHead(graph, e, out TVertex v))
                        continue;

                    if (!ExploredSetPolicy.Contains(exploredSet, v))
                    {
                        ExploredSetPolicy.Add(exploredSet, v);
                        yield return v;
                    }

                    stack.Add(outEdges);
                }
            }
            finally
            {
                stack.Dispose();
            }
        }
    }
    // ReSharper restore UnusedTypeParameter
}
