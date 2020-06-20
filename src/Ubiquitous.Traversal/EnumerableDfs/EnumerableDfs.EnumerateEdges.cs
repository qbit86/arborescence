namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    public readonly partial struct EnumerableDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy,
        TExploredSetPolicy>
    {
        public IEnumerator<TEdge> EnumerateEdges(TGraph graph, TVertex source, TExploredSet exploredSet)
        {
            var stack = new Internal.Stack<TVertex>();

            stack.Add(source);

            return EnumerateEdgesCore(graph, stack, exploredSet);
        }

        private IEnumerator<TEdge> EnumerateEdgesCore(
            TGraph graph, Internal.Stack<TVertex> stack, TExploredSet exploredSet)
        {
            try
            {
                while (stack.TryTake(out TVertex u))
                {
                    if (ExploredSetPolicy.Contains(exploredSet, u))
                        continue;

                    ExploredSetPolicy.Add(exploredSet, u);
                    TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(graph, u);
                    while (outEdges.MoveNext())
                    {
                        TEdge e = outEdges.Current;
                        if (!GraphPolicy.TryGetHead(graph, e, out TVertex v))
                            continue;

                        yield return e;
                        ExploredSetPolicy.Add(exploredSet, v);
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
