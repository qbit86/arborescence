namespace Ubiquitous.Traversal.Pragmatic
{
    using System;
    using System.Collections.Generic;

    public readonly struct BaselineIndexedBfs<TGraph, TEdge, TEdgeEnumerator, TGraphConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetTargetConcept<TGraph, int, TEdge>, IGetOutEdgesConcept<TGraph, int, TEdgeEnumerator>
    {
        public BaselineIndexedBfs(TGraphConcept graphConcept)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            GraphConcept = graphConcept;
        }

        public TGraphConcept GraphConcept { get; }

        public BaselineIndexedBfsCollection<TGraph, TEdge, TEdgeEnumerator, TGraphConcept> Traverse(
            TGraph graph, int startVertex, int vertexCount)
        {
            return new BaselineIndexedBfsCollection<TGraph, TEdge, TEdgeEnumerator, TGraphConcept>(
                graph, startVertex, vertexCount, GraphConcept);
        }
    }
}
