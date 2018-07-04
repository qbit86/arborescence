namespace Ubiquitous.Traversal.Pragmatic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    // https://github.com/boostorg/graph/blob/develop/include/boost/graph/breadth_first_search.hpp
    public readonly struct BaselineIndexedBfsCollection<TGraph, TEdge, TEdgeEnumerator, TGraphConcept>
        : IEnumerable<TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetTargetConcept<TGraph, int, TEdge>, IGetOutEdgesConcept<TGraph, int, TEdgeEnumerator>
    {
        internal BaselineIndexedBfsCollection(TGraph graph, int startVertex, int vertexCount, TGraphConcept graphConcept)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (startVertex < 0)
                throw new ArgumentOutOfRangeException(nameof(startVertex));

            if (vertexCount <= startVertex)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

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
            var colorMapConcept = new IndexedMapConcept<TGraph, Color>(VertexCount);
            var steps = new BaselineBfsCollection<TGraph, int, TEdge, TEdgeEnumerator, ArraySegment<Color>,
                TGraphConcept, IndexedMapConcept<TGraph, Color>>(Graph, StartVertex, GraphConcept, colorMapConcept);
            return steps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
