namespace Ubiquitous.Models
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct UncheckedEdgeIncidenceGraphPolicy<TGraph, TEdges> :
        IGetTailPolicy<TGraph, int, SourceTargetPair<int>>,
        IGetHeadPolicy<TGraph, int, SourceTargetPair<int>>,
        IOutEdgesPolicy<TGraph, int, TEdges>
        where TGraph : IIncidence<int, TEdges>
    {
        public bool TryGetTail(TGraph graph, SourceTargetPair<int> edge, out int tail)
        {
            tail = edge.Source;
            return true;
        }

        public bool TryGetHead(TGraph graph, SourceTargetPair<int> edge, out int head)
        {
            head = edge.Target;
            return true;
        }

        public TEdges EnumerateOutEdges(TGraph graph, int vertex) => graph.EnumerateOutEdges(vertex);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
