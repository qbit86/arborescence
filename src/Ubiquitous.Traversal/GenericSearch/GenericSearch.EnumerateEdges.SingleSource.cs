namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TVertexContainer,
        TExploredSet, TGraphPolicy, TVertexContainerPolicy, TExploredSetPolicy>
    {
        public IEnumerator<TEdge> EnumerateEdges(
            TGraph graph, TVertex source, TVertexContainer vertexContainer, TExploredSet exploredSet)
        {
            ExploredSetPolicy.Add(exploredSet, source);
            VertexContainerPolicy.Add(vertexContainer, source);

            return EnumerateEdgesCore(graph, vertexContainer, exploredSet);
        }
    }
    // ReSharper restore UnusedTypeParameter
}
