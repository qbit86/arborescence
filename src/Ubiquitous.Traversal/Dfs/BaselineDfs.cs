namespace Ubiquitous
{
    using System;
    using System.Collections.Generic;

    public struct BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TVertexConcept, TEdgeConcept, TColorMapFactory>

        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>

        where TVertexConcept : struct, IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TEdgeConcept : struct, IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
    {
        private TVertexConcept VertexConcept { get; }

        private TEdgeConcept EdgeConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        public BaselineDfs(TColorMapFactory colorMapFactory)
        {
            if (colorMapFactory == null)
                throw new ArgumentNullException(nameof(colorMapFactory));

            VertexConcept = default(TVertexConcept);
            EdgeConcept = default(TEdgeConcept);
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
            Traverse<TVertices>(TGraph graph, TVertices vertices)

            where TVertices : IEnumerator<TVertex>
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            return new BaselineDfsForestStepCollection<TGraph, TVertex, TEdge, TVertices, TEdgeEnumerator, TColorMap,
                TVertexConcept, TEdgeConcept, TColorMapFactory>(graph, vertices,
                VertexConcept, EdgeConcept, ColorMapFactory);
        }
    }
}
