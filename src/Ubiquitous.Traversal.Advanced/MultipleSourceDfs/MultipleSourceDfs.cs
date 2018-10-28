namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public struct MultipleSourceDfs<TGraph, TVertex, TEdge, TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
        TColorMap, TGraphConcept, TColorMapConcept, TVertexEnumerableConcept>
        where TVertexEnumerable : IEnumerable<TVertex>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
        where TVertexEnumerableConcept : IEnumerablePolicy<TVertexEnumerable, TVertexEnumerator>
    {
        private TGraphConcept GraphConcept { get; }

        private TColorMapConcept ColorMapConcept { get; }

        private TVertexEnumerableConcept VertexEnumerableConcept { get; }

        public MultipleSourceDfs(TGraphConcept graphConcept, TColorMapConcept colorMapConcept,
            TVertexEnumerableConcept vertexEnumerableConcept)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            if (colorMapConcept == null)
                throw new ArgumentNullException(nameof(colorMapConcept));

            if (vertexEnumerableConcept == null)
                throw new ArgumentNullException(nameof(vertexEnumerableConcept));

            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
            VertexEnumerableConcept = vertexEnumerableConcept;
        }

        public DfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TGraphConcept, TColorMapConcept, TVertexEnumerableConcept>
            Traverse(TGraph graph, TVertexEnumerable vertexCollection)
        {
            if (vertexCollection == null)
                throw new ArgumentNullException(nameof(vertexCollection));

            return new DfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TGraphConcept, TColorMapConcept, TVertexEnumerableConcept>(graph,
                vertexCollection, 0, GraphConcept, ColorMapConcept, VertexEnumerableConcept);
        }

        public DfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TGraphConcept, TColorMapConcept, TVertexEnumerableConcept>
            Traverse(TGraph graph, TVertexEnumerable vertexCollection, int stackCapacity)
        {
            if (vertexCollection == null)
                throw new ArgumentNullException(nameof(vertexCollection));

            return new DfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TGraphConcept, TColorMapConcept, TVertexEnumerableConcept>(graph,
                vertexCollection, stackCapacity, GraphConcept, ColorMapConcept, VertexEnumerableConcept);
        }
    }
}
