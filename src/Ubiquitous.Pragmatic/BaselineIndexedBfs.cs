namespace Ubiquitous
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
    }
}
