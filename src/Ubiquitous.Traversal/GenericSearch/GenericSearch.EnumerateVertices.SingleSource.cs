namespace Ubiquitous.Traversal
{
    using System.Collections.Generic;

    // ReSharper disable UnusedTypeParameter
    public readonly partial struct GenericSearch<TGraph, TVertex, TEdge, TEdgeEnumerator, TVertexContainer,
        TExploredSet, TGraphPolicy, TVertexContainerPolicy, TExploredSetPolicy>
    {
        public IEnumerator<TVertex> EnumerateVertices(
            TGraph graph, TVertex source, TVertexContainer vertexContainer, TExploredSet exploredSet)
        {
            ExploredSetPolicy.Add(exploredSet, source);
            yield return source;
            VertexContainerPolicy.Add(vertexContainer, source);

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
                        ExploredSetPolicy.Add(exploredSet, v);
                        yield return v;
                        VertexContainerPolicy.Add(vertexContainer, v);
                    }
                }
            }
        }
    }
    // ReSharper restore UnusedTypeParameter
}
