namespace Ubiquitous
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public readonly struct BaselineIndexedBfsCollection<TGraph, TEdge, TEdgeEnumerator, TGraphConcept>
        : IEnumerable<TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetTargetConcept<TGraph, int, TEdge>, IGetOutEdgesConcept<TGraph, int, TEdgeEnumerator>
    {
        public BaselineIndexedBfsCollection(TGraph graph, int startVertex, int vertexCount, TGraphConcept graphConcept)
        {
            Graph = graph;
            StartVertex = startVertex;
            VertexCount = vertexCount;
            GraphConcept = graphConcept;
        }

        public TGraph Graph { get; }
        public int StartVertex { get; }
        public int VertexCount { get; }
        public TGraphConcept GraphConcept { get; }

        public IEnumerator<TEdge> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
