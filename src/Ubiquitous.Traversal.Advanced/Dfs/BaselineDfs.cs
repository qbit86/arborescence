namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public struct BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphConcept, TColorMapFactory>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IMapConcept<TColorMap, TVertex, Color>, IFactory<TGraph, TColorMap>
    {
        private TGraphConcept GraphConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        public BaselineDfs(TGraphConcept graphConcept, TColorMapFactory colorMapFactory)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            if (colorMapFactory == null)
                throw new ArgumentNullException(nameof(colorMapFactory));

            GraphConcept = graphConcept;
            ColorMapFactory = colorMapFactory;
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new BaselineDfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapFactory>(graph, startVertex, GraphConcept, ColorMapFactory);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse<TVertexEnumerator>(TGraph graph, TVertexEnumerator vertexEnumerator)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (vertexEnumerator == null)
                throw new ArgumentNullException(nameof(vertexEnumerator));

            return new BaselineDfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TGraphConcept, TColorMapFactory>(graph, vertexEnumerator, GraphConcept, ColorMapFactory);
        }
    }
}
