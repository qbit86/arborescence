namespace Arborescence.Traversal.Incidence
{
    using System.Collections.Generic;
    using System.Threading;

    public static partial class RecursiveDfs<TVertex, TEdge, TEdgeEnumerator>
    {
        private static void TraverseUnchecked<TGraph, TColorMap, THandler>(
            TGraph graph, TVertex source, TColorMap colorByVertex, THandler handler,
            CancellationToken cancellationToken)
            where TGraph : IHeadIncidence<TVertex, TEdge>, IOutEdgesIncidence<TVertex, TEdgeEnumerator>
            where TColorMap : IDictionary<TVertex, Color>
            where THandler : IDfsHandler<TGraph, TVertex, TEdge>
        {
            handler.OnStartVertex(graph, source);
            TraverseCore(graph, source, colorByVertex, handler, cancellationToken);
        }
    }
}
