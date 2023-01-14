namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public static partial class EnumerableGenericSearch<TVertex, TNeighborEnumerator>
    {
        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerableGenericSearch.EnumerateVerticesChecked<
                TVertex, TNeighborEnumerator, TGraph, TFrontier, TExploredSet>(graph, source, frontier, exploredSet);
    }
}
