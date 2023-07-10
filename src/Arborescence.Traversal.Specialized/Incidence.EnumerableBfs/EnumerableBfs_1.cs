namespace Arborescence.Traversal.Specialized.Incidence
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class EnumerableBfs<TEdge>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, IEnumerator<TEdge>> =>
            EnumerableBfs<TEdge, IEnumerator<TEdge>>.EnumerateVerticesChecked(graph, source, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, IEnumerator<TEdge>>
            where TSourceCollection : IEnumerable<int> =>
            EnumerableBfs<TEdge, IEnumerator<TEdge>>.EnumerateVerticesChecked(graph, sources, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TEdge> EnumerateEdges<TGraph>(TGraph graph, int source, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, IEnumerator<TEdge>> =>
            EnumerableBfs<TEdge, IEnumerator<TEdge>>.EnumerateEdgesChecked(graph, source, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TSourceCollection>(
            TGraph graph, TSourceCollection sources, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, IEnumerator<TEdge>>
            where TSourceCollection : IEnumerable<int> =>
            EnumerableBfs<TEdge, IEnumerator<TEdge>>.EnumerateEdgesChecked(graph, sources, vertexCount);
    }
}
