namespace Arborescence.Models
{
    using System.Collections.Generic;

    public readonly struct EdgeIdentityIncidenceGraph<TVertex, TEdge, TEdgeEnumerator> :
        ITailIncidence<TVertex, TEdge>,
        IHeadIncidence<TVertex, TEdge>,
        IOutEdgesIncidence<TVertex, TEdgeEnumerator>,
        IAdjacency<TVertex, IEnumerator<TVertex>>
    {
        public bool TryGetTail(TEdge edge, out TVertex tail) => throw new System.NotImplementedException();

        public bool TryGetHead(TEdge edge, out TVertex head) => throw new System.NotImplementedException();

        public TEdgeEnumerator EnumerateOutEdges(TVertex vertex) => throw new System.NotImplementedException();

        public IEnumerator<TVertex> EnumerateNeighbors(TVertex vertex) => throw new System.NotImplementedException();
    }
}
