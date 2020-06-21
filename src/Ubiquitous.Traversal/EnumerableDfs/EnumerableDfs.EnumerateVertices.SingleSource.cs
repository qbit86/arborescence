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

            stack.Add(new VertexInfo(source, false));

            return EnumerateVerticesCore(graph, stack, exploredSet);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
