namespace Ubiquitous.Traversal.Advanced.MultipleSourceDfs
{
    using System;
    using System.Collections.Generic;

    public struct MultipleSourceDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TVertexEnumerable, TVertexEnumerator,
        TColorMap, TGraphConcept, TColorMapConcept, TVertexEnumerableConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TVertexEnumerable : IEnumerable<TVertex>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TColorMap>
        where TVertexEnumerableConcept : IEnumerableConcept<TVertexEnumerable, TVertexEnumerator>
    {
        private TGraphConcept GraphConcept { get; }

        private TColorMapConcept ColorMapConcept { get; }

        private TVertexEnumerableConcept VertexCollectionConcept { get; }

        public MultipleSourceDfs(TGraphConcept graphConcept, TColorMapConcept colorMapConcept,
            TVertexEnumerableConcept vertexCollectionConcept)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            if (colorMapConcept == null)
                throw new ArgumentNullException(nameof(colorMapConcept));

            if (vertexCollectionConcept == null)
                throw new ArgumentNullException(nameof(vertexCollectionConcept));

            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
            VertexCollectionConcept = vertexCollectionConcept;
        }

        public DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>
            Traverse(TGraph graph, TVertexEnumerable vertexCollection)
        {
            if (vertexCollection == null)
                throw new ArgumentNullException(nameof(vertexCollection));

            TVertexEnumerator vertexEnumerator = VertexCollectionConcept.GetEnumerator(vertexCollection);
            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TGraphConcept, TColorMapConcept>(graph,
                vertexEnumerator, 0, GraphConcept, ColorMapConcept);
        }

        public DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>
            Traverse(TGraph graph, TVertexEnumerable vertexCollection, int stackCapacity)
        {
            if (vertexCollection == null)
                throw new ArgumentNullException(nameof(vertexCollection));

            TVertexEnumerator vertexEnumerator = VertexCollectionConcept.GetEnumerator(vertexCollection);
            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TGraphConcept, TColorMapConcept>(graph,
                vertexEnumerator, stackCapacity, GraphConcept, ColorMapConcept);
        }
    }
}
