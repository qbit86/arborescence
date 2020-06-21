namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        private IEnumerator<TEdge> EnumerateEdgesCore(
            TGraph graph, Internal.Stack<EdgeInfo> stack, TExploredSet exploredSet)
        {
            try
            {
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

        private readonly struct EdgeInfo
        {
            private readonly TVertex _exploredVertex;
            private readonly TEdge _inEdge;
            private readonly bool _hasInEdge;

            internal EdgeInfo(TVertex exploredVertex)
            {
                _exploredVertex = exploredVertex;
                _inEdge = default;
                _hasInEdge = false;
            }

            internal EdgeInfo(TVertex exploredVertex, TEdge inEdge)
            {
                _exploredVertex = exploredVertex;
                _inEdge = inEdge;
                _hasInEdge = true;
            }

            internal TVertex ExploredVertex => _exploredVertex;

            internal bool TryGetInEdge(out TEdge inEdge)
            {
                inEdge = _inEdge;
                return _hasInEdge;
            }
        }
    }
    // ReSharper restore UnusedTypeParameter
}
