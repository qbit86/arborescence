namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static partial class EnumerableBfs
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TVertex> EnumerateVertices<
            TVertex, TNeighborEnumerator, TGraph, TExploredSet>(TGraph graph, TVertex source, TExploredSet exploredSet)
            where TNeighborEnumerator : IEnumerator<TVertex>
            where TGraph : IAdjacency<TVertex, TNeighborEnumerator>
            where TExploredSet : ISet<TVertex> =>
            EnumerableBfs<TVertex, TNeighborEnumerator>.EnumerateVerticesChecked(
                graph, source, exploredSet);
    }
}
