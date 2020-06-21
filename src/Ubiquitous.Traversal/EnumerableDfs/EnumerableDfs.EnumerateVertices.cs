namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        private IEnumerator<TVertex> EnumerateVerticesCore(
            TGraph graph, Internal.Stack<VertexInfo> stack, TExploredSet exploredSet)
        {
            try
            {
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

        private readonly struct VertexInfo
        {
            private readonly TVertex _exploredVertex;
            private readonly bool _hasInEdge;

            internal VertexInfo(TVertex exploredVertex, bool hasInEdge)
            {
                _exploredVertex = exploredVertex;
                _hasInEdge = hasInEdge;
            }

            internal TVertex ExploredVertex => _exploredVertex;
            internal bool HasInEdge => _hasInEdge;
        }
    }
    // ReSharper restore UnusedTypeParameter
}
