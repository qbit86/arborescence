namespace Ubiquitous.Traversal.Pragmatic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    // https://github.com/boostorg/graph/blob/develop/include/boost/graph/breadth_first_search.hpp
    public readonly struct BaselineIndexedBfsCollection<TGraph, TEdge, TEdgeEnumerator, TGraphPolicy>
        : IEnumerable<TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetTargetPolicy<TGraph, int, TEdge>, IGetOutEdgesPolicy<TGraph, int, TEdgeEnumerator>
    {
        private readonly BaselineBfsCollection<TGraph, int, TEdge, TEdgeEnumerator, ArraySegment<Color>,
            TGraphPolicy, IndexedMapPolicy<Color>> _impl;

        internal BaselineIndexedBfsCollection(TGraph graph, int startVertex, int vertexCount, int queueCapacity,
            TGraphPolicy graphPolicy)
        {
            var colorMapPolicy = new IndexedMapPolicy<Color>(vertexCount);
            _impl = new BaselineBfsCollection<TGraph, int, TEdge, TEdgeEnumerator, ArraySegment<Color>,
                TGraphPolicy, IndexedMapPolicy<Color>>(
                graph, startVertex, queueCapacity, graphPolicy, colorMapPolicy);
        }

        public IEnumerator<TEdge> GetEnumerator()
        {
            return _impl.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
