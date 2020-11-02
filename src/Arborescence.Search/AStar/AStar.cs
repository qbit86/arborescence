namespace Arborescence.Search
{
    using System.Collections.Generic;

    // https://boost.org/doc/libs/1_74_0/libs/graph/doc/astar_search.html
    // https://boost.org/doc/libs/1_74_0/libs/graph/doc/AStarHeuristic.html

    public readonly struct AStar<TGraph, TEdge, TEdgeEnumerator>
        where TGraph : IOutEdgesConcept<int, TEdgeEnumerator>, IHeadConcept<int, TEdge>
        where TEdgeEnumerator : IEnumerator<TEdge> { }
}
