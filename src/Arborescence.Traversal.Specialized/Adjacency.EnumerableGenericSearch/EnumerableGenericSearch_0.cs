namespace Arborescence.Traversal.Specialized.Adjacency
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class EnumerableGenericSearch
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph, TFrontier>(
            TGraph graph, int source, TFrontier frontier, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, IEnumerator<int>>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerableGenericSearch<IEnumerator<int>>.EnumerateVerticesChecked(graph, source, frontier, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> EnumerateVertices<TGraph, TSourceCollection, TFrontier>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, IEnumerator<int>>
            where TSourceCollection : IEnumerable<int>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerableGenericSearch<IEnumerator<int>>.EnumerateVerticesChecked(graph, sources, frontier, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<int>> EnumerateEdges<TGraph, TFrontier>(
            TGraph graph, int source, TFrontier frontier, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, IEnumerator<int>>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerableGenericSearch<IEnumerator<int>>.EnumerateEdgesChecked(graph, source, frontier, vertexCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Endpoints<int>> EnumerateEdges<TGraph, TSourceCollection, TFrontier>(
            TGraph graph, TSourceCollection sources, TFrontier frontier, int vertexCount)
            where TGraph : IOutNeighborsAdjacency<int, IEnumerator<int>>
            where TSourceCollection : IEnumerable<int>
            where TFrontier : IProducerConsumerCollection<int> =>
            EnumerableGenericSearch<IEnumerator<int>>.EnumerateEdgesChecked(graph, sources, frontier, vertexCount);
    }
}
