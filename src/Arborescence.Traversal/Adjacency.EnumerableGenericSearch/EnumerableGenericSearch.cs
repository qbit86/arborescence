namespace Arborescence.Traversal.Adjacency
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public static class EnumerableGenericSearch
    {
        public static IEnumerator<TVertex> EnumerateVertices<
            TVertex, TEdge, TVertexEnumerator, TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TVertexEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerateVerticesChecked<TVertex, TEdge, TVertexEnumerator, TGraph, TFrontier, TExploredSet>(
                graph, source, frontier, exploredSet);

        internal static IEnumerator<TVertex> EnumerateVerticesChecked<
            TVertex, TEdge, TVertexEnumerator, TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TVertexEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            if (graph is null)
                ThrowHelper.ThrowArgumentNullException(nameof(graph));

            if (frontier is null)
                ThrowHelper.ThrowArgumentNullException(nameof(frontier));

            if (exploredSet is null)
                ThrowHelper.ThrowArgumentNullException(nameof(exploredSet));

            return EnumerateVerticesIterator<TVertex, TEdge, TVertexEnumerator, TGraph, TFrontier, TExploredSet>(
                graph, source, frontier, exploredSet);
        }

        internal static IEnumerator<TVertex> EnumerateVerticesIterator<
            TVertex, TEdge, TVertexEnumerator, TGraph, TFrontier, TExploredSet>(
            TGraph graph, TVertex source, TFrontier frontier, TExploredSet exploredSet)
            where TVertexEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TVertexEnumerator>
            where TFrontier : IProducerConsumerCollection<TVertex>
            where TExploredSet : ISet<TVertex> =>
            throw new NotImplementedException();
    }
}
