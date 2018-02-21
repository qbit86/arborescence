namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public struct BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactory>

        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
    {
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        public BaselineDfs(TVertexConcept vertexConcept, TEdgeConcept edgeConcept, TColorMapFactory colorMapFactory)
        {
            if (vertexConcept == null)
                throw new ArgumentNullException(nameof(vertexConcept));

            if (edgeConcept == null)
                throw new ArgumentNullException(nameof(edgeConcept));

            if (colorMapFactory == null)
                throw new ArgumentNullException(nameof(colorMapFactory));

            VertexConcept = vertexConcept;
            EdgeConcept = edgeConcept;
            ColorMapFactory = colorMapFactory;
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new BaselineDfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactory>(graph, startVertex,
                VertexConcept, EdgeConcept, ColorMapFactory);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse<TVertexEnumerator>(TGraph graph, TVertexEnumerator vertexEnumerator)

            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (vertexEnumerator == null)
                throw new ArgumentNullException(nameof(vertexEnumerator));

            return new BaselineDfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactory>(graph, vertexEnumerator,
                VertexConcept, EdgeConcept, ColorMapFactory);
        }
    }
}
