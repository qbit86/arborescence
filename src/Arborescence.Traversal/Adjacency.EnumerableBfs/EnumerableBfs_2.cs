namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;

    public static class EnumerableBfs<TVertex, TNeighborEnumerator>
        where TNeighborEnumerator : IEnumerator<TVertex>
    {
        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex> =>
            EnumerableBfs.EnumerateVerticesChecked<TVertex, TNeighborEnumerator, TGraph, TExploredSet>(
                graph, source, exploredSet);

        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerableBfs.EnumerateVerticesChecked<
                TVertex, TNeighborEnumerator, TGraph, TSourceEnumerator, TExploredSet>(graph, sources, exploredSet);
    }
}
