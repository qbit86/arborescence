namespace Ubiquitous.Traversal.Advanced
{
    using System;
    using System.Collections.Generic;

    public struct Dfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
        TGraphConcept, TColorMapFactory>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TColorMap : IDictionary<TVertex, Color>
        where TGraphConcept : IGetOutEdgesConcept<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetConcept<TGraph, TVertex, TEdge>
        where TColorMapFactory : IFactory<TGraph, TColorMap>
    {
        private TGraphConcept GraphConcept { get; }

        private TColorMapFactory ColorMapFactory { get; }

        public Dfs(TGraphConcept graphConcept, TColorMapFactory colorMapFactory)
        {
            if (graphConcept == null)
                throw new ArgumentNullException(nameof(graphConcept));

            if (colorMapFactory == null)
                throw new ArgumentNullException(nameof(colorMapFactory));

            GraphConcept = graphConcept;
            ColorMapFactory = colorMapFactory;
        }

        public DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapFactory>
            Traverse(TGraph graph, TVertex startVertex)
        {
            return new DfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapFactory>(graph, startVertex,
                GraphConcept, ColorMapFactory);
        }

        public DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator, TColorMap,
                TGraphConcept, TColorMapFactory>
            Traverse<TVertexEnumerator>(TGraph graph, TVertexEnumerator vertexEnumerator)
            where TVertexEnumerator : IEnumerator<TVertex>
        {
            if (vertexEnumerator == null)
                throw new ArgumentNullException(nameof(vertexEnumerator));

            return new DfsForestStepCollection<TGraph, TVertex, TEdge, TVertexEnumerator, TEdgeEnumerator,
                TColorMap, TGraphConcept, TColorMapFactory>(graph,
                vertexEnumerator, GraphConcept, ColorMapFactory);
        }
    }
}
