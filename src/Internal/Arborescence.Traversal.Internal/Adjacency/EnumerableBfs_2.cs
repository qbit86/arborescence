namespace Arborescence.Traversal.Internal.Adjacency
{
    using System.Collections.Generic;

    internal static class EnumerableBfs<TVertex, TNeighborEnumerator>
        where TNeighborEnumerator : IEnumerator<TVertex>
    {
        internal static IEnumerable<TVertex> EnumerateVerticesIterator<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            using Traversal.Queue<TVertex> frontier = new();
            var vertices = EnumerableGenericSearch<TVertex, TNeighborEnumerator>
                .EnumerateVerticesIterator(graph, source, frontier, exploredSet);
            foreach (var vertex in vertices)
                yield return vertex;
        }

        internal static IEnumerable<TVertex> EnumerateVerticesIterator<TGraph, TSourceCollection, TExploredSet>(
            TGraph graph, TSourceCollection sources, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            using Traversal.Queue<TVertex> frontier = new();
            var vertices = EnumerableGenericSearch<TVertex, TNeighborEnumerator>
                .EnumerateVerticesIterator(graph, sources, frontier, exploredSet);
            foreach (var vertex in vertices)
                yield return vertex;
        }

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesIterator<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            using Traversal.Queue<TVertex> frontier = new();
            var edges = EnumerableGenericSearch<TVertex, TNeighborEnumerator>
                .EnumerateEdgesIterator(graph, source, frontier, exploredSet);
            foreach (var edge in edges)
                yield return edge;
        }

        internal static IEnumerable<Endpoints<TVertex>> EnumerateEdgesIterator<
            TGraph, TSourceCollection, TExploredSet>(
            TGraph graph, TSourceCollection sources, TExploredSet exploredSet)
            where TGraph : IOutNeighborsAdjacency<TVertex, TNeighborEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            using Traversal.Queue<TVertex> frontier = new();
            var edges = EnumerableGenericSearch<TVertex, TNeighborEnumerator>
                .EnumerateEdgesIterator(graph, sources, frontier, exploredSet);
            foreach (var edge in edges)
                yield return edge;
        }
    }
}
