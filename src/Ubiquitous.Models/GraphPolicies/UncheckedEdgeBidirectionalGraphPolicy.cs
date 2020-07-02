namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct UncheckedEdgeBidirectionalGraphPolicy<TGraph, TEdges> :
        ITailPolicy<TGraph, int, Endpoints<int>>,
        IHeadPolicy<TGraph, int, Endpoints<int>>,
        IOutEdgesPolicy<TGraph, int, TEdges>,
        IInEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IInEdgesConcept<int, TEdges>, IOutEdgesConcept<int, TEdges>
    {
        public bool TryGetTail(TGraph graph, Endpoints<int> edge, out int tail)
        {
            tail = edge.Tail;
            return true;
        }

        public bool TryGetHead(TGraph graph, Endpoints<int> edge, out int head)
        {
            head = edge.Head;
            return true;
        }

        public TEdges EnumerateOutEdges(TGraph graph, int vertex) => graph.EnumerateOutEdges(vertex);

        public TEdges EnumerateInEdges(TGraph graph, int vertex) => graph.EnumerateInEdges(vertex);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
