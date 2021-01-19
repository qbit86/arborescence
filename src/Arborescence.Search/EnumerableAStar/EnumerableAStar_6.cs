namespace Arborescence.Search
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;

    public readonly struct EnumerableAStar<TGraph, TEdge, TEdgeEnumerator, TCost, TCostComparer, TCostMonoidPolicy>
        where TGraph : IIncidenceGraph<int, TEdge, TEdgeEnumerator>
        where TEdgeEnumerator : IEnumerator<TEdge>
        where TCostComparer : IComparer<TCost>
        where TCostMonoidPolicy : IMonoidPolicy<TCost>
    {
        private readonly TCostComparer _costComparer;
        private readonly TCostMonoidPolicy _costMonoidPolicy;

        public EnumerableAStar(TCostComparer costComparer, TCostMonoidPolicy costMonoidPolicy)
        {
            if (costComparer == null)
                throw new ArgumentNullException(nameof(costComparer));

            if (costMonoidPolicy == null)
                throw new ArgumentNullException(nameof(costMonoidPolicy));

            _costComparer = costComparer;
            _costMonoidPolicy = costMonoidPolicy;
        }

        public IEnumerator<TEdge> EnumerateRelaxedEdges(TGraph graph, int source, int vertexCount)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (vertexCount < 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount));

            if (unchecked((uint)source >= (uint)vertexCount))
                yield break;

            byte[] colorMap = ArrayPool<byte>.Shared.Rent(vertexCount);
            Array.Clear(colorMap, 0, colorMap.Length);
            try
            {
                throw new NotImplementedException();
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(colorMap);
            }
        }
    }
}
