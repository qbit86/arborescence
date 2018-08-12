namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public struct Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphConcept, TColorMapConcept>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapConcept : IMapConcept<TColorMap, TVertex, Color>, IFactory<TColorMap>
    {
        private TGraphConcept GraphConcept { get; }

        private TColorMapConcept ColorMapConcept { get; }

        public Dfs(TGraphConcept graphConcept, TColorMapConcept colorMapConcept)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            if (colorMapConcept == null)
                throw new ArgumentNullException(nameof(colorMapConcept));

            GraphConcept = graphConcept;
            ColorMapConcept = colorMapConcept;
        }

        public DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>(graph, startVertex, 0,
                GraphConcept, ColorMapConcept);
        }

        public DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>
            Traverse(TGraph graph, TVertex startVertex, int stackCapacity)
        {
            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapConcept>(graph, startVertex, stackCapacity,
                GraphConcept, ColorMapConcept);
        }
    }
}
