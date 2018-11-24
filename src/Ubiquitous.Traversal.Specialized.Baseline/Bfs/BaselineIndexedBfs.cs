namespace Ubiquitous.Traversal.Pragmatic
{
    using System;
    using System.Collections.Generic;

    public readonly struct BaselineIndexedBfs<TGraph, TEdge, TEdgeEnumerator, TGraphPolicy>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetTargetPolicy<TGraph, int, TEdge>, IGetOutEdgesPolicy<TGraph, int, TEdgeEnumerator>
    {
        public BaselineIndexedBfs(TGraphPolicy graphPolicy)
        {
            if (graphPolicy == null)
                throw new ArgumentNullException(nameof(graphPolicy));

            GraphPolicy = graphPolicy;
        }

        private TGraphPolicy GraphPolicy { get; }

        public BaselineIndexedBfsCollection<TGraph, TEdge, TEdgeEnumerator, TGraphPolicy> Traverse(
            TGraph graph, int startVertex, int vertexUpperBound)
        {
            return new BaselineIndexedBfsCollection<TGraph, TEdge, TEdgeEnumerator, TGraphPolicy>(
                graph, startVertex, vertexUpperBound, 0, GraphPolicy);
        }

        public BaselineIndexedBfsCollection<TGraph, TEdge, TEdgeEnumerator, TGraphPolicy> Traverse(
            TGraph graph, int startVertex, int vertexUpperBound, int queueCapacity)
        {
            return new BaselineIndexedBfsCollection<TGraph, TEdge, TEdgeEnumerator, TGraphPolicy>(
                graph, startVertex, vertexUpperBound, queueCapacity, GraphPolicy);
        }
    }
}
