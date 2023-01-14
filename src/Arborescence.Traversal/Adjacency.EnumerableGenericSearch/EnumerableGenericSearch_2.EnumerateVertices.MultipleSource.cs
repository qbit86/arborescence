namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public static partial class EnumerableGenericSearch<TVertex, TNeighborEnumerator>
    {
        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TSourceEnumerator, TFrontier, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> => EnumerableGenericSearch
            .EnumerateVerticesChecked<TVertex, TNeighborEnumerator, TGraph, TSourceEnumerator, TFrontier, TExploredSet>(
                graph, sources, frontier, exploredSet);
    }
}
