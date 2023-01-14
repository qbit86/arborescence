namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;

    public static partial class EnumerableBfs<TVertex>
    {
        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TSourceEnumerator : IEnumerator<TVertex> =>
            EnumerableBfs<TVertex, IEnumerator<TVertex>>.EnumerateVerticesChecked(graph, sources);

        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TSourceEnumerator>(
            TGraph graph, TSourceEnumerator sources, IEqualityComparer<TVertex> comparer)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TSourceEnumerator : IEnumerator<TVertex> =>
            EnumerableBfs<TVertex, IEnumerator<TVertex>>.EnumerateVerticesChecked(graph, sources, comparer);

        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TSourceEnumerator, TExploredSet>(
            TGraph graph, TSourceEnumerator sources, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, IEnumerator<TVertex>>
            where TSourceEnumerator : IEnumerator<TVertex>
            where TExploredSet : ISet<TVertex> =>
            EnumerableBfs<TVertex, IEnumerator<TVertex>>.EnumerateVerticesChecked(graph, sources, exploredSet);
    }
}
