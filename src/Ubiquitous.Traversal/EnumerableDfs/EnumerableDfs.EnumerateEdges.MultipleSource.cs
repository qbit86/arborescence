namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        public IEnumerator<TEdge> EnumerateEdges<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator sources, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            var stack = new Internal.Stack<EdgeInfo>();

            while (sources.MoveNext())
            {
                TVertex source = sources.Current;
                stack.Add(new EdgeInfo(source));
            }

            return EnumerateEdgesCore(graph, stack, exploredSet);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
