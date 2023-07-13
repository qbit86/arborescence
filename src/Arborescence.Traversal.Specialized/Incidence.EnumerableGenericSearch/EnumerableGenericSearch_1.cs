namespace Arborescence.Traversal.Specialized.Incidence
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class EnumerableGenericSearch<TEdge>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph, TFrontier>(
            TGraph graph, int source, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, IEnumerator<TEdge>>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerableGenericSearch<TEdge, IEnumerator<TEdge>>.EnumerateVerticesChecked(
                graph, source, frontier, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph, TSourceCollection, TFrontier>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, IEnumerator<TEdge>>
            where TSourceCollection : IEnumerable<int>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerableGenericSearch<TEdge, IEnumerator<TEdge>>.EnumerateVerticesChecked(
                graph, sources, frontier, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TFrontier>(
            TGraph graph, int source, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, IEnumerator<TEdge>>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerableGenericSearch<TEdge, IEnumerator<TEdge>>.EnumerateEdgesChecked(
                graph, source, frontier, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TEdge> EnumerateEdges<TGraph, TSourceCollection, TFrontier>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, int vertexCount)
            where TGraph : IHeadIncidence<int, TEdge>, IOutEdgesIncidence<int, IEnumerator<TEdge>>
            where TSourceCollection : IEnumerable<int>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerableGenericSearch<TEdge, IEnumerator<TEdge>>.EnumerateEdgesChecked(
                graph, sources, frontier, vertexCount);
    }
}
