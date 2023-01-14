namespace Arborescence.Traversal.Adjacency
{
    using System.Collections.Generic;

    public static class EnumerableBfs<TVertex, TVertexEnumerator>
        where TVertexEnumerator : IEnumerator<TVertex>
    {
        public static IEnumerator<TVertex> EnumerateVertices<TGraph, TExploredSet>(
            TGraph graph, TVertex source, TExploredSet exploredSet)
            where TGraph : IAdjacency<TVertex, TVertexEnumerator>
            where TExploredSet : ISet<TVertex> =>
            EnumerableBfs.EnumerateVerticesChecked<TVertex, TVertexEnumerator, TGraph, TExploredSet>(
                graph, source, exploredSet);
    }
}
