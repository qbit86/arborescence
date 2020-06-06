namespace Ubiquitous.Traversal
{
    using System;
    using System.Collections.Generic;

    public static class BaselineUndirectedDfs<TGraph, TVertex, TEdge, TEdgeEnumerator,
        TVertexColorMap, TEdgeColorMap, TStep>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        public static BaselineUndirectedDfs<TGraph, TVertex, TEdge, TEdgeEnumerator,
                TVertexColorMap, TEdgeColorMap, TStep,
                TGraphPolicy, TVertexColorMapPolicy, TEdgeColorMapPolicy, TStepPolicy>
#pragma warning disable CA1000 // Do not declare static members on generic types
            Create<TGraphPolicy, TVertexColorMapPolicy, TEdgeColorMapPolicy, TStepPolicy>(
                TGraphPolicy graphPolicy, TVertexColorMapPolicy vertexColorMapPolicy,
                TEdgeColorMapPolicy edgeColorMapPolicy, TStepPolicy stepPolicy)
            where TGraphPolicy :
            IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IInEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
            IGetHeadPolicy<TGraph, TVertex, TEdge>, IGetTailPolicy<TGraph, TVertex, TEdge>
            where TVertexColorMapPolicy : IMapPolicy<TVertexColorMap, TVertex, Color>
            where TEdgeColorMapPolicy : IMapPolicy<TEdgeColorMap, TEdge, Color>
            where TStepPolicy : IUndirectedStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
#pragma warning restore CA1000 // Do not declare static members on generic types
        {
            return new BaselineUndirectedDfs<TGraph, TVertex, TEdge, TEdgeEnumerator, TVertexColorMap, TEdgeColorMap,
                TStep, TGraphPolicy, TVertexColorMapPolicy, TEdgeColorMapPolicy, TStepPolicy>(
                graphPolicy, vertexColorMapPolicy, edgeColorMapPolicy, stepPolicy);
        }
    }

#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct BaselineUndirectedDfs<TGraph, TVertex, TEdge, TEdgeEnumerator,
        TVertexColorMap, TEdgeColorMap, TStep,
        TGraphPolicy, TVertexColorMapPolicy, TEdgeColorMapPolicy, TStepPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy :
        IOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>, IInEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetHeadPolicy<TGraph, TVertex, TEdge>, IGetTailPolicy<TGraph, TVertex, TEdge>
        where TVertexColorMapPolicy : IMapPolicy<TVertexColorMap, TVertex, Color>
        where TEdgeColorMapPolicy : IMapPolicy<TEdgeColorMap, TEdge, Color>
        where TStepPolicy : IUndirectedStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
#pragma warning restore CA1815 // Override equals and operator equals on value types
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

        public IEnumerable<TStep> Traverse(TGraph graph, TVertex startVertex,
            TVertexColorMap vertexColorMap, TEdgeColorMap edgeColorMap)
        {
            return new BaselineUndirectedDfsTreeStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator,
                TVertexColorMap, TEdgeColorMap, TStep,
                TGraphPolicy, TVertexColorMapPolicy, TEdgeColorMapPolicy, TStepPolicy>(
                graph, startVertex, vertexColorMap, edgeColorMap,
                _graphPolicy, _vertexColorMapPolicy, _edgeColorMapPolicy, _stepPolicy);
        }
    }
}
