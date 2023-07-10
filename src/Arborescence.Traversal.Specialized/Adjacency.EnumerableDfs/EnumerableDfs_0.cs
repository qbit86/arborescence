namespace Arborescence.Traversal.Specialized.Adjacency
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class EnumerableDfs
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, IEnumerator<int>> =>
            EnumerableDfs<IEnumerator<int>>.EnumerateVerticesChecked(graph, source, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, IEnumerator<int>>
            where TSourceCollection : IEnumerable<int> =>
            EnumerableDfs<IEnumerator<int>>.EnumerateVerticesChecked(graph, sources, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<int>> EnumerateEdges<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, IEnumerator<int>> =>
            EnumerableDfs<IEnumerator<int>>.EnumerateEdgesChecked(graph, source, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<int>> EnumerateEdges<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, IEnumerator<int>>
            where TSourceCollection : IEnumerable<int> =>
            EnumerableDfs<IEnumerator<int>>.EnumerateEdgesChecked(graph, sources, vertexCount);
    }
}
