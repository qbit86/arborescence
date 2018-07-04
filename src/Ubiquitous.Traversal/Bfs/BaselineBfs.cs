namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly struct BaselineBfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphConcept, TColorMapConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetTargetConcept<TGraph, TVertex, TEdge>,
        IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>
        where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TGraph, TColorMap>
    {
        public BaselineBfs(TGraphConcept graphConcept, TColorMapConcept colorMapConcept)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
        }

        public TGraphConcept GraphConcept { get; }
        public TColorMapConcept ColorMapConcept { get; }

        public BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new BaselineBfsCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>(graph, startVertex, GraphConcept, ColorMapConcept);
        }
    }
}
