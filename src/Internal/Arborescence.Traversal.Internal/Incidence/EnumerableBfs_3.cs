namespace Arborescence.Traversal.Internal.Incidence
{
    using System.Collections.Generic;

    internal static class EnumerableBfs<TVertex, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
    {
        internal static IEnumerable<TVertex> EnumerateVerticesIterator<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            using Traversal.Queue<TVertex> frontier = new();
            var vertices = EnumerableGenericSearch<TVertex, TEdge, TEdgeEnumerator>
                .EnumerateVerticesIterator(graph, source, frontier, exploredSet);
            foreach (var vertex in vertices)
                yield return vertex;
        }

        internal static IEnumerable<TVertex> EnumerateVerticesIterator<TGraph, TSourceCollection, TExploredSet>(
            TGraph graph, TSourceCollection sources, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            using Traversal.Queue<TVertex> frontier = new();
            var vertices = EnumerableGenericSearch<TVertex, TEdge, TEdgeEnumerator>
                .EnumerateVerticesIterator(graph, sources, frontier, exploredSet);
            foreach (var vertex in vertices)
                yield return vertex;
        }

        internal static IEnumerable<TEdge> EnumerateEdgesIterator<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TExploredSet : ISet<TVertex>
        {
            using Traversal.Queue<TVertex> frontier = new();
            var edges = EnumerableGenericSearch<TVertex, TEdge, TEdgeEnumerator>
                .EnumerateEdgesIterator(graph, source, frontier, exploredSet);
            foreach (var edge in edges)
                yield return edge;
        }

        internal static IEnumerable<TEdge> EnumerateEdgesIterator<
            TGraph, TSourceCollection, TExploredSet>(
            TGraph graph, TSourceCollection sources, TExploredSet exploredSet)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TSourceCollection : IEnumerable<TVertex>
            where TExploredSet : ISet<TVertex>
        {
            using Traversal.Queue<TVertex> frontier = new();
            var edges = EnumerableGenericSearch<TVertex, TEdge, TEdgeEnumerator>
                .EnumerateEdgesIterator(graph, sources, frontier, exploredSet);
            foreach (var edge in edges)
                yield return edge;
        }
    }
}
