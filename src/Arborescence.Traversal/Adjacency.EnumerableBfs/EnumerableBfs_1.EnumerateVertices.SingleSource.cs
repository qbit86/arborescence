namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;

    public static partial class EnumerableBfs<TVertex>
    {
        public static IEnumerator<TVertex> EnumerateVertices<TGraph>(TGraph graph, TVertex source)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>> =>
            EnumerableBfs<TVertex, IEnumerator<TVertex>>.EnumerateVerticesChecked(graph, source);

        public static IEnumerator<TVertex> EnumerateVertices<TGraph>(
            TGraph graph, TVertex source, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>> =>
            EnumerableBfs<TVertex, IEnumerator<TVertex>>.EnumerateVerticesChecked(graph, source, comparer);

        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TExploredSet : ISet<TVertex> =>
            EnumerableBfs<TVertex, IEnumerator<TVertex>>.EnumerateVerticesChecked(graph, source, exploredSet);
    }
}
