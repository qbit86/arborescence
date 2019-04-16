namespace Ubiquitous.Traversal
{
    using System.Collections;
    using System.Collections.Generic;

    internal struct BaselineUndirectedDfsStepCollection<TGraph, TVertex, TEdge, TEdgeEnumerator,
        TVertexColorMap, TEdgeColorMap, TStep,
        TGraphPolicy, TVertexColorMapPolicy, TEdgeColorMapPolicy, TStepPolicy> : IEnumerable<TStep>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetOutEdgesPolicy<TGraph, TVertex, TEdgeEnumerator>,
        IGetTargetPolicy<TGraph, TVertex, TEdge>
        where TVertexColorMapPolicy : IMapPolicy<TVertexColorMap, TVertex, Color>
        where TEdgeColorMapPolicy : IMapPolicy<TEdgeColorMap, TEdge, Color>
        where TStepPolicy : IStepPolicy<DfsStepKind, TVertex, TEdge, TStep>
    {
        internal BaselineUndirectedDfsStepCollection(TGraph graph, TVertex startVertex,
            TVertexColorMap vertexColorMap, TEdgeColorMap edgeColorMap,
            TGraphPolicy graphPolicy, TVertexColorMapPolicy vertexColorMapPolicy,
            TEdgeColorMapPolicy edgeColorMapPolicy, TStepPolicy stepPolicy)
        {

        }

        public IEnumerator<TStep> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
