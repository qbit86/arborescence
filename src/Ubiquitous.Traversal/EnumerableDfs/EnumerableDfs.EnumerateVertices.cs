namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy,
        TExploredSetPolicy>
    {
        public IEnumerator<TVertex> EnumerateVertices(TGraph graph, TVertex source, TExploredSet exploredSet)
        {
            var stack = new Internal.Stack<TVertex>();

            stack.Add(source);

            return EnumerateVerticesCore(graph, stack, exploredSet);
        }

        public IEnumerator<TVertex> EnumerateVertices<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator sources, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            var stack = new Internal.Stack<TVertex>();

            while (sources.MoveNext())
            {
                TVertex s = sources.Current;
                stack.Add(s);
            }

            return EnumerateVerticesCore(graph, stack, exploredSet);
        }

        private IEnumerator<TVertex> EnumerateVerticesCore(
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

                        yield return v;
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
    // ReSharper restore UnusedTypeParameter
}
