namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public static class EnumerableGenericSearch<TVertex, TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<TVertex>
    {
        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TVertexEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerableGenericSearch.EnumerateVerticesChecked<
                TVertex, TVertexEnumerator, TGraph, TFrontier, TExploredSet>(graph, source, frontier, exploredSet);

        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TSourceEnumerator, TFrontier, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TFrontier frontier, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TVertexEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> => EnumerableGenericSearch
            .EnumerateVerticesChecked<TVertex, TVertexEnumerator, TGraph, TSourceEnumerator, TFrontier, TExploredSet>(
                graph, sources, frontier, exploredSet);
    }
}
