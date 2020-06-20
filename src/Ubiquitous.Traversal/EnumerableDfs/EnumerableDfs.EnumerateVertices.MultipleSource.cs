namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct EnumerableDfs<
        TGraph, TVertex, TEdge, TEdgeEnumerator, TExploredSet, TGraphPolicy, TExploredSetPolicy>
    {
        public IEnumerator<TVertex> EnumerateVertices<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator sources, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            var stack = new Internal.Stack<VertexInfo>();

            while (sources.MoveNext())
            {
                TVertex source = sources.Current;
                stack.Add(new VertexInfo(source, false));
            }

            return EnumerateVerticesCore(graph, stack, exploredSet);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
