namespace Arborescence.Models
{
    using System;

    public sealed class MutableUndirectedSimpleIncidenceGraph :
        IIncidenceGraph<int, Endpoints, ArrayPrefixEnumerator<Endpoints>>,
        IGraphBuilder<SimpleIncidenceGraph, int, Endpoints>,
        IDisposable
    {
        private const int DefaultInitialOutDegree = 8;

        private int _edgeCount;
        private int _initialOutDegree = DefaultInitialOutDegree;
        private ArrayPrefix<ArrayPrefix<Endpoints>> _outEdgesByVertex;

        public void Dispose() => throw new NotImplementedException();

        public bool TryAdd(int tail, int head, out Endpoints edge) => throw new NotImplementedException();

        public SimpleIncidenceGraph ToGraph() => throw new NotImplementedException();
        public bool TryGetHead(Endpoints edge, out int head) => throw new NotImplementedException();

        public bool TryGetTail(Endpoints edge, out int tail) => throw new NotImplementedException();

        public ArrayPrefixEnumerator<Endpoints> EnumerateOutEdges(int vertex) => throw new NotImplementedException();
    }
}
