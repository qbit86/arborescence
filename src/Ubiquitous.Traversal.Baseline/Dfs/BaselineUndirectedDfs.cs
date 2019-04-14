namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

    public readonly struct BaselineUndirectedDfs<TGraph, TVertex, TEdge, TEdgeEnumerator,
        TVertexColorMap, TEdgeColorMap, TStep,
        TGraphPolicy, TVertexColorMapPolicy, TEdgeColorMapPolicy, TStepPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetInEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetTargetPolicy<TGraph, TVertex, TEdge>,
            IGetSourcePolicy<TGraph, TVertex, TEdge>
        where TVertexColorMapPolicy : IMapPolicy<TVertexColorMap, TVertex, Color>, IFactory<TVertexColorMap>
        where TEdgeColorMapPolicy : IMapPolicy<TEdgeColorMap, TEdge, Color>, IFactory<TVertexColorMap>
        where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
    {
        private readonly TGraphPolicy _graphPolicy;
        private readonly TVertexColorMapPolicy _vertexColorMapPolicy;
        private readonly TEdgeColorMapPolicy _edgeColorMapPolicy;
        private readonly TStepPolicy _stepPolicy;

        public BaselineUndirectedDfs(TGraphPolicy graphPolicy, TVertexColorMapPolicy vertexColorMapPolicy,
            TEdgeColorMapPolicy edgeColorMapPolicy, TStepPolicy stepPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            if (vertexColorMapPolicy == null)
                throw new ArgumentNullException(nameof(vertexColorMapPolicy));

            if (edgeColorMapPolicy == null)
                throw new ArgumentNullException(nameof(edgeColorMapPolicy));

            if (stepPolicy == null)
                throw new ArgumentNullException(nameof(stepPolicy));

            _graphPolicy = graphPolicy;
            _vertexColorMapPolicy = vertexColorMapPolicy;
            _edgeColorMapPolicy = edgeColorMapPolicy;
            _stepPolicy = stepPolicy;
        }
    }
}
