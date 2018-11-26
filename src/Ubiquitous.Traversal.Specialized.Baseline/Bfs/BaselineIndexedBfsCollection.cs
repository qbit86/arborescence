namespace Ubiquitous.Traversal.Pragmatic
{
    using System.Collections;
    using System.Collections.Generic;
    using Models;

    // https://github.com/boostorg/graph/blob/develop/include/boost/graph/breadth_first_search.hpp
    public readonly struct BaselineIndexedBfsCollection<TGraph, TEdge, TEdgeEnumerator, TGraphPolicy>
        : IEnumerable<TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TGraphPolicy : IGetTargetPolicy<TGraph, int, TEdge>, IGetOutEdgesPolicy<TGraph, int, TEdgeEnumerator>
    {
        private readonly BaselineBfsCollection<TGraph, int, TEdge, TEdgeEnumerator, ArrayPrefix<Color>,
            TGraphPolicy, IndexedMapPolicy<Color>> _impl;

        internal BaselineIndexedBfsCollection(TGraph graph, int startVertex, int vertexUpperBound, int queueCapacity,
            TGraphPolicy graphPolicy)
        {
            var colorMapPolicy = new IndexedMapPolicy<Color>(vertexUpperBound);
            _impl = new BaselineBfsCollection<TGraph, int, TEdge, TEdgeEnumerator, ArrayPrefix<Color>,
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
