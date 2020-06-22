namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        public IEnumerator<TEdge> EnumerateEdges(TGraph graph, TVertex source, TExploredSet exploredSet)
        {
            var stack = new Internal.Stack<EdgeInfo>();
            try
            {
                stack.Add(new EdgeInfo(source));

                while (stack.TryTake(out EdgeInfo stackFrame))
                {
                    TVertex u = stackFrame.ExploredVertex;
                    if (ExploredSetPolicy.Contains(exploredSet, u))
                        continue;

                    if (stackFrame.TryGetInEdge(out TEdge inEdge))
                        yield return inEdge;
                    ExploredSetPolicy.Add(exploredSet, u);

                    TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(graph, u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;
                        if (!GraphPolicy.TryGetHead(graph, e, out TVertex v))
                            continue;

                        stack.Add(new EdgeInfo(v, e));
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
