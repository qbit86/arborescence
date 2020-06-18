namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TVertexContainer,
        TExploredSet, TGraphPolicy, TVertexContainerPolicy, TExploredSetPolicy>
    {
        public IEnumerator<TEdge> EnumerateEdges<TVertexEnumerator>(
            TGraph graph, TVertexEnumerator sources, TVertexContainer vertexContainer, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            while (sources.MoveNext())
            {
                TVertex source = sources.Current;
                ExploredSetPolicy.Add(exploredSet, source);
                VertexContainerPolicy.Add(vertexContainer, source);
            }

            return EnumerateEdgesCore(graph, vertexContainer, exploredSet);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
