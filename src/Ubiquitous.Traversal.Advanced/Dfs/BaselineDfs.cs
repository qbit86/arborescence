namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public struct BaselineDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphConcept, TColorMapConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TColorMap>
    {
        private TGraphConcept GraphConcept { get; }

        private TColorMapConcept ColorMapConcept { get; }

        public BaselineDfs(TGraphConcept graphConcept, TColorMapConcept colorMapConcept)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            if (colorMapConcept == null)
                throw new ArgumentNullException(nameof(colorMapConcept));

            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new BaselineDfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>(graph, startVertex, GraphConcept, ColorMapConcept);
        }

        public IEnumerable<Step<DfsStepKind, TVertex, TEdge>>
            Traverse<TVertexEnumerator>(TGraph graph, TVertexEnumerator vertexEnumerator)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (vertexEnumerator == null)
                throw new ArgumentNullException(nameof(vertexEnumerator));

            return new BaselineDfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TGraphConcept, TColorMapConcept>(graph, vertexEnumerator, GraphConcept, ColorMapConcept);
        }
    }
}
