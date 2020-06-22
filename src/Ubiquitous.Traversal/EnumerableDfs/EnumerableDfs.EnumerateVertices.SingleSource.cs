namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        public IEnumerator<TVertex> EnumerateVertices(TGraph graph, TVertex source, TExploredSet exploredSet)
        {
            var stack = new Internal.Stack<VertexInfo>();
            try
            {
                stack.Add(new VertexInfo(source, false));

                while (stack.TryTake(out VertexInfo stackFrame))
                {
                    TVertex u = stackFrame.ExploredVertex;
                    if (ExploredSetPolicy.Contains(exploredSet, u))
                        continue;

                    ExploredSetPolicy.Add(exploredSet, u);
                    if (stackFrame.HasInEdge)
                        yield return u;

                    TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(graph, u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;
                        if (!GraphPolicy.TryGetHead(graph, e, out TVertex v))
                            continue;

                        stack.Add(new VertexInfo(v, true));
                    }
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
