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

            stack.Add(new EdgeInfo(source));

            return EnumerateEdgesCore(graph, stack, exploredSet);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
