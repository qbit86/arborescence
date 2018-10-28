namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly struct BaselineBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphConcept, TColorMapConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetTargetPolicy<TGraph, TVertex, TEdge>,
        IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>
        where TColorMapConcept : IMapPolicy<TColorMap, TVertex, Color>, IFactory<TColorMap>
    {
        public BaselineBfs(TGraphConcept graphConcept, TColorMapConcept colorMapConcept)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
        }

        private TGraphConcept GraphConcept { get; }
        private TColorMapConcept ColorMapConcept { get; }

        public BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>(graph, startVertex, 0, GraphConcept, ColorMapConcept);
        }

        public BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>
            Traverse(TGraph graph, TVertex startVertex, int queueCapacity)
        {
            return new BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>(graph, startVertex, queueCapacity, GraphConcept, ColorMapConcept);
        }
    }
}
