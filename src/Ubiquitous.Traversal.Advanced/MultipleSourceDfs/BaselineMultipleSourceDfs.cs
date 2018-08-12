namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public struct BaselineMultipleSourceDfs<TGraph, TVertex, TEdge,
        TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator, TColorMap,
        TGraphConcept, TColorMapConcept, TVertexEnumerableConcept>
        where TVertexEnumerable : IEnumerable<TVertex>
        where TVertexEnumerator : IEnumerator<TVertex>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TColorMap>
        where TVertexEnumerableConcept : IEnumerableConcept<TVertexEnumerable, TVertexEnumerator>
    {
        private TGraphConcept GraphConcept { get; }

        private TColorMapConcept ColorMapConcept { get; }

        private TVertexEnumerableConcept VertexEnumerableConcept { get; }

        public BaselineMultipleSourceDfs(TGraphConcept graphConcept, TColorMapConcept colorMapConcept,
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

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse(TGraph graph, TVertexEnumerable vertexCollection)
        {
            if (vertexCollection == null)
                throw new ArgumentNullException(nameof(vertexCollection));

            return new BaselineDfsForestStepCollection<TGraph, TVertex, TEdge,
                TVertexEnumerable, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TGraphConcept, TColorMapConcept, TVertexEnumerableConcept>(graph, vertexCollection,
                GraphConcept, ColorMapConcept, VertexEnumerableConcept);
        }
    }
}
