namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TVertexContainer,
        TExploredSet, TGraphPolicy, TVertexContainerPolicy, TExploredSetPolicy>
    {
        private IEnumerator<TEdge> EnumerateEdgesCore(
            TGraph graph, TVertexContainer vertexContainer, TExploredSet exploredSet)
        {
            while (VertexContainerPolicy.TryTake(vertexContainer, out TVertex u))
            {
                TEdgeEnumerator outEdges = GraphPolicy.EnumerateOutEdges(graph, u);
                while (outEdges.MoveNext())
                {
                    TEdge e = outEdges.Current;
                    if (!GraphPolicy.TryGetHead(graph, e, out TVertex v))
                        continue;

                    bool vExplored = ExploredSetPolicy.Contains(exploredSet, v);
                    if (!vExplored)
                    {
                        yield return e;
                        ExploredSetPolicy.Add(exploredSet, v);
                        VertexContainerPolicy.Add(vertexContainer, v);
                    }
                }
            }
        }
    }
    // ReSharper restore UnusedTypeParameter
}
